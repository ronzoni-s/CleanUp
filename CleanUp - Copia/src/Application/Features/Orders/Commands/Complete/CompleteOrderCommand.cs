using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Application.Interfaces.Services;
using ErbertPranzi.Application.Requests;
using ErbertPranzi.Domain.Entities.Catalog;
using ErbertPranzi.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ErbertPranzi.Shared.Constants.Application;
using System.Text;
using ErbertPranzi.Shared.Settings;
using System.IO;
using Microsoft.Extensions.Logging;
using ErbertPranzi.Application.Enums;

namespace ErbertPranzi.Application.Features.Orders.Commands.Complete
{
    public partial class CompleteOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int UsedBags { get; set; }
    }

    internal class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, Result<int>>
    {
        private readonly ILogger<CompleteOrderCommand> logger;
        //private readonly PrinterSettings printerSettings;
        //private readonly ReceiptPrinterSettings receiptPrinterSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<CompleteOrderCommandHandler> _localizer;
        private readonly IParameterRepository _parameterRepository;
        private readonly IOrderRepository orderRepository;

        public CompleteOrderCommandHandler(
            IUnitOfWork<int> unitOfWork
            , IMapper mapper
            , IUploadService uploadService
            , IStringLocalizer<CompleteOrderCommandHandler> localizer
            , IParameterRepository parameterRepository
            , ILogger<CompleteOrderCommand> logger
            , IOrderRepository orderRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _parameterRepository = parameterRepository;
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<int>> Handle(CompleteOrderCommand command, CancellationToken cancellationToken)
        {

            return await Result<int>.SuccessAsync(1, _localizer["Order Completed"]);
        }
    }
}