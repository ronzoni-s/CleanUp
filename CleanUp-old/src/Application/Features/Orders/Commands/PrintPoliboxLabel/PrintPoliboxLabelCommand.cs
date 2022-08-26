using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Application.Interfaces.Services;
using CleanUp.Application.Requests;
using CleanUp.Domain.Entities.Catalog;
using CleanUp.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using CleanUp.Shared.Constants.Application;
using System.Text;
using CleanUp.Shared.Settings;
using System.IO;
using Microsoft.Extensions.Logging;
using CleanUp.Application.Enums;

namespace CleanUp.Application.Features.Orders.Commands
{
    public partial class PrintPoliboxLabelCommand : IRequest<IResult>
    {
        public int OrderId { get; set; }
        public int PoliboxNumber { get; set; }
        public bool PrintPolibox { get; set; }
        public List<int> Bags { get; set; }
    }

    internal class PrintPoliboxLabelCommandHandler : IRequestHandler<PrintPoliboxLabelCommand, IResult>
    {
        private readonly ILogger<PrintPoliboxLabelCommand> logger;
        private readonly IUnitOfWork<int> unitOfWork;
        private readonly IStringLocalizer<PrintPoliboxLabelCommandHandler> localizer;
        private readonly IOrderRepository orderRepository;

        public PrintPoliboxLabelCommandHandler(
            IUnitOfWork<int> unitOfWork
            , IStringLocalizer<PrintPoliboxLabelCommandHandler> localizer
            , IParameterRepository parameterRepository
            , ILogger<PrintPoliboxLabelCommand> logger
            , IOrderRepository orderRepository)
        {
            this.unitOfWork = unitOfWork;
            this.localizer = localizer;
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<IResult> Handle(PrintPoliboxLabelCommand command, CancellationToken cancellationToken)
        {
            try
            {
                return await Result.SuccessAsync(localizer["Order Completed"]);
            } 
            catch (Exception e)
            {
                throw;
            }
        }
    }
}