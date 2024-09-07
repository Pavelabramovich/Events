using Events.Application.Dto;
using FluentValidation;


namespace Events.WebApi.Validators;


public class UserCreatingDtoValidator : AbstractValidator<UserCreatingDto>
{
    public UserCreatingDtoValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(5, 32).WithMessage("Name must be between 5 and 32 characters.");

        RuleFor(user => user.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .Length(5, 32).WithMessage("Surname must be between 5 and 32 characters.");

        RuleFor(user => user.Login)
            .NotEmpty().WithMessage("Login is required.")
            .Length(5, 32).WithMessage("Login must be between 5 and 32 characters.");

        RuleFor(user => user.HashedPassword)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(5).WithMessage("Your password length must be at least 5.")
            .MaximumLength(32).WithMessage("Your password length must not exceed 32.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
    }
}