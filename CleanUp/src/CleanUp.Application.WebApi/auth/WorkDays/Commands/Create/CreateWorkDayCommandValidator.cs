using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Application.WebApi.WorkDays.Commands
{
    public class CreateWorkDayCommandValidator : AbstractValidator<CreateWorkDayCommand>
    {
        public CreateWorkDayCommandValidator()
        {
        }
    }
}
