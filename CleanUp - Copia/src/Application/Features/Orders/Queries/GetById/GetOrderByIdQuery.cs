using AutoMapper;
using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Domain.Entities.Catalog;
using ErbertPranzi.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ErbertPranzi.Application.Features.Orders.Queries.GetById
{
    public class GetOrderByIdQuery : IRequest<Result<GetOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<GetOrderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IParameterRepository parameterRepository;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IOrderRepository orderRepository, IParameterRepository parameterRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.orderRepository = orderRepository;
            this.parameterRepository = parameterRepository;
        }

        public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<Order, GetOrderByIdResponse>> expression = e => new GetOrderByIdResponse
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                CustomerAddressId = e.CustomerAddressId,
                CustomerName = e.CustomerAddress.Customer.Name,
                CustomerAddress = e.CustomerAddress.Address,
                ContactName = e.ContactName,
                ContactPhoneNumber = e.ContactPhoneNumber,
                OrderDate = e.OrderDate,
                Bags = e.Bags,
                UsedBags = e.UsedBags,
                Weight = e.Weight,
                BagsPerPolibox = e.BagsPerPolibox,
                WeightPerBag = e.WeightPerBag,
                FinalPrice = e.FinalPrice,
                CompletionDateTime = e.CompletionDateTime,
                CancellationDateTime = e.CancellationDateTime,
                OrderProducts = e.OrderProducts.Select(op => new GetOrderProductResponse
                {
                    Id = op.Product.Id,
                    Code = op.Product.Code,
                    Name = op.Product.Name,
                    Weight = op.ProductWeight,
                    Price = op.Price,
                    FinalPrice = op.FinalPrice,
                    Tax = op.Tax,
                    Quantity = op.Quantity,
                    IsActive = op.Product.IsActive,
                    MenuId = op.MenuId,
                }).ToList()
            };

            var order = _unitOfWork.Repository<Order>().Entities
                            .Select(expression)
                            .FirstOrDefault(x => x.Id == query.Id);

            order.OrderPoliboxs = await orderRepository.GetOrderPoliboxs(query.Id);

            return await Result<GetOrderByIdResponse>.SuccessAsync(order);
        }
    }
}