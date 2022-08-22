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
using ErbertPranzi.Domain.Entities;

namespace ErbertPranzi.Application.Features.Emails.Commands.UpdateLastEmailDateTime
{
    public partial class UpdateLastEmailDateTimeCommand : IRequest<Result<int>>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime? LastDateTime { get; set; }
    }

    internal class UpdateLastEmailDateTimeCommandHandler : IRequestHandler<UpdateLastEmailDateTimeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<UpdateLastEmailDateTimeCommandHandler> _localizer;

        public UpdateLastEmailDateTimeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<UpdateLastEmailDateTimeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateLastEmailDateTimeCommand command, CancellationToken cancellationToken)
        {
            var email = await _unitOfWork.Repository<EmailConfig>().GetByIdAsync(command.Id);
            if (email != null)
            {
                email.LastEmailDateTime = command.LastDateTime;
                await _unitOfWork.Repository<EmailConfig>().UpdateAsync(email);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(email.Id, _localizer["Email Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Email Not Found!"]);
            }
        }
    }
}