using ErbertPranzi.Application.Features.Orders.Commands.AddEdit;
using ErbertPranzi.Application.Features.Orders.Queries.GetById;
using ErbertPranzi.Application.Features.Orders.Queries.GetAllPaged;
using ErbertPranzi.Application.Requests.Catalog;
using ErbertPranzi.Shared.Wrapper;
using System.Threading.Tasks;
using ErbertPranzi.Application.Features.Orders.Commands.Complete;
using ErbertPranzi.Application.Features.Orders.Commands.Void;
using ErbertPranzi.Application.Features.Orders.Commands;

namespace ErbertPranzi.Client.Infrastructure.Managers.Catalog.Order
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