using CleanUp.Application.Features.Orders.Commands.Complete;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanUp.Application.Validators.Features.Orders.Commands.Complete
{
    public class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
    {
        public CompleteOrderCommandValidator(IStringLocalizer<CompleteOrderCommandValidator> localizer)
        {
            
        }
    }
}