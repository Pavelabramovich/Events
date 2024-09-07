using Events.Application.Dto;
using FluentValidation;


namespace Events.Application.Validators;


public class EventCreatingDtoValidator : AbstractValidator<EventCreatingDto>
{
    public EventCreatingDtoValidator()
    {
        RuleFor(@event => @event.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(5).WithMessage("Event name length must be at least 5.")
            .MaximumLength(32).WithMessage("Event name length must not exceed 32.");

        RuleFor(@event => @event.Description)
            .MaximumLength(1024).WithMessage("Event description length must not exceed 1024.");

        RuleFor(@event => @event.Category)
            .IsInEnum().WithMessage("Unknown category type.");

        RuleFor(@event => @event.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MinimumLength(5).WithMessage("Address length must be at least 5.")
            .MaximumLength(128).WithMessage("Address length must not exceed 32.");
    }
}
