using ErbertPranzi.Application.Features.Orders.Commands.Complete;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ErbertPranzi.Application.Validators.Features.Orders.Commands.Complete
{
    public class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
    {
        public CompleteOrderCommandValidator(IStringLocalizer<CompleteOrderCommandValidator> localizer)
        {
            
        }
    }
}