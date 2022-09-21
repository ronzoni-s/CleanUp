using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(x => x.ClassroomId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
        }
    }
}
