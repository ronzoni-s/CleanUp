using CleanUp.Application.Extensions;
using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Application.Specifications.Catalog;
using CleanUp.Domain.Entities.Catalog;
using CleanUp.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CleanUp.Application.Features.Orders.Queries.GetAllPaged
{
    public class GetAllOrdersQuery : IRequest<PaginatedResult<GetAllPagedOrdersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...
        public bool HideCompleted { get; set; }
        public bool HideVoided { get; set; }

        public GetAllOrdersQuery(int pageNumber, int pageSize, string searchString, string orderBy, bool hideCompleted, bool hideVoided)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            HideCompleted = hideCompleted;
            HideVoided = hideVoided;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PaginatedResult<GetAllPagedOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllOrdersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedOrdersResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Order, GetAllPagedOrdersResponse>> expression = e => new GetAllPagedOrdersResponse
            {
                Id = e.Id,
                OrderNumber = e.OrderNumber,
                CustomerAddressId = e.CustomerAddressId,
                CustomerName = e.CustomerAddress.Customer.Name,
                CustomerAddress = e.CustomerAddress.Address,
                ContactName = e.ContactName,
                ContactPhoneNumber = e.ContactPhoneNumber,
                OrderDate = e.OrderDate,
                FinalPrice = e.FinalPrice,
                Bags = e.Bags,
                CompletionDateTime = e.CompletionDateTime,
                CancellationDateTime = e.CancellationDateTime
            };
            var orderFilterSpec = new OrderFilterSpecification(request.SearchString, request.HideCompleted, request.HideVoided);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Order>().Entities
                   .Specify(orderFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Order>().Entities
                   .Specify(orderFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}