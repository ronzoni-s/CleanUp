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
using CleanUp.Application.Features.Products.Queries.GetAllPaged;

namespace CleanUp.Application.Features.Products.Queries.GetOrderProductsPaged
{
    public class GetOrderProductsQuery : IRequest<PaginatedResult<GetAllPagedOrderProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public int? OrderId { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetOrderProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy, int? orderId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            OrderId = orderId;
        }
    }

    internal class GetOrderProductsQueryHandler : IRequestHandler<GetOrderProductsQuery, PaginatedResult<GetAllPagedOrderProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetOrderProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedOrderProductsResponse>> Handle(GetOrderProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<OrderProduct, GetAllPagedOrderProductsResponse>> expression = e => new GetAllPagedOrderProductsResponse
            {
                Id = e.Id,
                OrderId = e.OrderId,
                ProductId = e.ProductId,
                ProductWeight = e.ProductWeight,
                Price = e.Price,
                FinalPrice = e.FinalPrice,
                Tax = e.Tax,
                Quantity = e.Quantity,
                MenuId = e.MenuId,
                Product = new GetAllPagedProductsResponse
                {
                    Id = e.Product.Id,
                    Code = e.Product.Code,
                    Name = e.Product.Name,
                    IsActive = e.Product.IsActive,
                    Price = e.Product.Price,
                    Tax = e.Product.Tax,
                    Weight = e.Product.Weight
                }
            };
            var orderProductFilterSpec = new OrderProductFilterSpecification(request.SearchString, request.OrderId);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<OrderProduct>().Entities
                   .Specify(orderProductFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<OrderProduct>().Entities
                   .Specify(orderProductFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}