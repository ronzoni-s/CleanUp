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
    public class GetNextOrderIdQuery : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class GetNextOrderIdQueryHandler : IRequestHandler<GetNextOrderIdQuery, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IParameterRepository parameterRepository;

        public GetNextOrderIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IOrderRepository orderRepository, IParameterRepository parameterRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.orderRepository = orderRepository;
            this.parameterRepository = parameterRepository;
        }

        public async Task<Result<int>> Handle(GetNextOrderIdQuery query, CancellationToken cancellationToken)
        {
            var order = _unitOfWork.Repository<Order>().Entities
                            .OrderBy(x => x.Id)
                            .FirstOrDefault(x => x.Id > query.Id);

            if (order == null)
                return await Result<int>.FailAsync();            

            return await Result<int>.SuccessAsync(order.Id);
        }
    }
}