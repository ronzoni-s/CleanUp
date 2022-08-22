using CleanUp.Application.Features.Products.Commands.AddEdit;
using CleanUp.Application.Features.Products.Commands.Update;
using CleanUp.Application.Features.Products.Queries.GetAllPaged;
using CleanUp.Application.Features.Products.Queries.GetOrderProductsPaged;
using CleanUp.Application.Requests.Catalog;
using CleanUp.Shared.Wrapper;
using System.Threading.Tasks;

namespace CleanUp.Client.Infrastructure.Managers.Catalog.Product
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