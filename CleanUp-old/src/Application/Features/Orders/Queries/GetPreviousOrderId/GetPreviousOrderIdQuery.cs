using AutoMapper;
using CleanUp.Application.Features.Products.Queries.GetAllPaged;
using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Domain.Entities.Catalog;
using CleanUp.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CleanUp.Application.Features.Orders.Queries.GetById
{
    public class GetPreviousOrderIdQuery : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class GetPreviousOrderIdQueryHandler : IRequestHandler<GetPreviousOrderIdQuery, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IParameterRepository parameterRepository;

        public GetPreviousOrderIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IOrderRepository orderRepository, IParameterRepository parameterRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.orderRepository = orderRepository;
            this.parameterRepository = parameterRepository;
        }

        public async Task<Result<int>> Handle(GetPreviousOrderIdQuery query, CancellationToken cancellationToken)
        {
            var order = _unitOfWork.Repository<Order>().Entities
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefault(x => x.Id < query.Id);

            if (order == null)
                return await Result<int>.FailAsync();            

            return await Result<int>.SuccessAsync(order.Id);
        }
    }
}