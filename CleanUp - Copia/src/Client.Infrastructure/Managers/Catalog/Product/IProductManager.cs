using ErbertPranzi.Application.Features.Products.Commands.AddEdit;
using ErbertPranzi.Application.Features.Products.Commands.Update;
using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using ErbertPranzi.Application.Features.Products.Queries.GetOrderProductsPaged;
using ErbertPranzi.Application.Requests.Catalog;
using ErbertPranzi.Shared.Wrapper;
using System.Threading.Tasks;

namespace ErbertPranzi.Client.Infrastructure.Managers.Catalog.Product
{
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request);
        
        Task<PaginatedResult<GetAllPagedOrderProductsResponse>> GetOrderProductsAsync(GetAllPagedOrderProductsRequest request);

        Task<IResult<string>> GetProductImageAsync(int id);

        Task<IResult<int>> SaveAsync(UpdateProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}