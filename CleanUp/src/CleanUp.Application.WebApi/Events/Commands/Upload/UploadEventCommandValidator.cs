using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class UploadEventCommandValidator : AbstractValidator<UploadEventCommand>
    {
        public UploadEventCommandValidator()
        {
            //RuleFor(x => x.File)
            //    .NotNull();
        }
    }
}
