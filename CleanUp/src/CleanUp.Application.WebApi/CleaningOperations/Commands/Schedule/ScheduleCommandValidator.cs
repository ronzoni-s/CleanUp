using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.CleaningOperations.Commands
{
    public class ScheduleCommandValidator : AbstractValidator<ScheduleCommand>
    {
        public ScheduleCommandValidator()
        {
        }
    }
}
