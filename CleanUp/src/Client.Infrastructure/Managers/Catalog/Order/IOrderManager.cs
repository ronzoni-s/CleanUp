using CleanUp.Application.Features.Orders.Commands.AddEdit;
using CleanUp.Application.Features.Orders.Queries.GetById;
using CleanUp.Application.Features.Orders.Queries.GetAllPaged;
using CleanUp.Application.Requests.Catalog;
using CleanUp.Shared.Wrapper;
using System.Threading.Tasks;
using CleanUp.Application.Features.Orders.Commands.Complete;
using CleanUp.Application.Features.Orders.Commands.Void;
using CleanUp.Application.Features.Orders.Commands;

namespace CleanUp.Client.Infrastructure.Managers.Catalog.Order
{
    public interface IOrderManager : IManager
    {
        Task<PaginatedResult<GetAllPagedOrdersResponse>> GetOrdersAsync(GetAllPagedOrdersRequest request);

        Task<IResult<GetOrderByIdResponse>> GetOrderByIdAsync(int id);
        Task<IResult<int>> GetNextOrderId(int id);
        Task<IResult<int>> GetPreviousOrderId(int id);

        Task<IResult<int>> SaveAsync(AddOrderCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult> PrintPoliboxLabelAsync(PrintPoliboxLabelCommand request);
        Task<IResult> PrintProductsAsync();
        Task<IResult<int>> CompleteAsync(CompleteOrderCommand request);
        Task<IResult<int>> VoidAsync(VoidOrderCommand request);
    }
}