using FluentValidation;

namespace CleanUp.Application.WebApi.Events.Commands
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
        }
    }
}
