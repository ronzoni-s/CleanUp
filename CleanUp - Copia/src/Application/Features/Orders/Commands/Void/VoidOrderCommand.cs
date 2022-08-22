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

namespace ErbertPranzi.Application.Features.Orders.Commands.Void
{
    public partial class VoidOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class VoidOrderCommandHandler : IRequestHandler<VoidOrderCommand, Result<int>>
    {
        private readonly ILogger<VoidOrderCommand> logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<VoidOrderCommandHandler> _localizer;
        private readonly IParameterRepository _parameterRepository;

        public VoidOrderCommandHandler(
            IUnitOfWork<int> unitOfWork
            , IMapper mapper
            , IUploadService uploadService
            , IStringLocalizer<VoidOrderCommandHandler> localizer
            , IParameterRepository parameterRepository
            , ILogger<VoidOrderCommand> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _parameterRepository = parameterRepository;
            this.logger = logger;
        }

        public async Task<Result<int>> Handle(VoidOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                
                return await Result<int>.SuccessAsync(0, _localizer["Order Voided"]);
            } 
            catch (Exception e)
            {
                throw;
            }
        }

    }
}