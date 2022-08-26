using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Domain.Entities.Catalog;
using CleanUp.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace CleanUp.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteOrderCommandHandler> _localizer;

        public DeleteOrderCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(command.Id);
            if (order != null)
            {
                await _unitOfWork.Repository<Order>().DeleteAsync(order);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(order.Id, _localizer["Order Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Order Not Found!"]);
            }
        }
    }
}