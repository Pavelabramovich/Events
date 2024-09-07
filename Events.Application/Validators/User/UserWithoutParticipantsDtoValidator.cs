using Events.Application.Dto;
using FluentValidation;


namespace Events.WebApi.Validators;


public class UserWithoutParticipantsDtoValidator : AbstractValidator<UserWithoutParticipantsDto>
{
    public UserWithoutParticipantsDtoValidator()
    {
        RuleFor(user => user.Name)
           .NotEmpty().WithMessage("Name is required.")
           .Length(5, 32).WithMessage("Name must be between 5 and 32 characters.");

        RuleFor(user => user.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .Length(5, 32).WithMessage("Surname must be between 5 and 32 characters.");
    }
}