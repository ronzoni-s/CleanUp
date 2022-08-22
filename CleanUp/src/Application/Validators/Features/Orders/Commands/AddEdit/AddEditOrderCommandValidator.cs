using CleanUp.Application.Features.Orders.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanUp.Application.Validators.Features.Orders.Commands.AddEdit
{
    public class AddEditOrderCommandValidator : AbstractValidator<AddOrderCommand>
    {
        public AddEditOrderCommandValidator(IStringLocalizer<AddEditOrderCommandValidator> localizer)
        {
            RuleFor(request => request.OrderNumber)
                .NotEmpty().WithMessage(x => localizer["OrderNumber is required!"]);
            RuleFor(request => request.CustomerName)
                .NotEmpty().WithMessage(x => localizer["CustomerName is required!"]);
            RuleFor(request => request.CustomerAddress)
                .NotEmpty().WithMessage(x => localizer["CustomerAddress is required!"]);
            RuleFor(request => request.ContactName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ContactName is required!"]);
            RuleFor(request => request.ContactPhoneNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ContactPhoneNumber is required!"]);

            //When(request => request.Bags.HasValue || request.CompletionDateTime.HasValue, () => {
            //    RuleFor(request => request.Bags)
            //        .NotNull().GreaterThanOrEqualTo(0).WithMessage(x => localizer["Bags is required!"]);
            //    RuleFor(request => request.WeightPerBag)
            //        .GreaterThanOrEqualTo(0).WithMessage(x => localizer["WeightPerBag is required!"]);
            //    RuleFor(request => request.BagsPerPolibox)
            //        .NotNull().GreaterThanOrEqualTo(0).WithMessage(x => localizer["BagsPerPolibox is required!"]);
            //    RuleFor(request => request.CompletionDateTime)
            //        .NotNull().WithMessage(x => localizer["CompletionDateTime is required!"]);
            //});

            RuleForEach(request => request.OrderProducts)
                .NotNull()
                .ChildRules(child => 
                {
                    child.RuleFor(x => x.ProductCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["ProductCode is required!"]);
                    child.RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage(x => localizer["Quantity must be positive!"]);
                });
            
        }
    }
}