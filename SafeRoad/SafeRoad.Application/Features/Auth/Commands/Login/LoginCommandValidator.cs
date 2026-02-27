using FluentValidation;

namespace SafeRoad.Core.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>//Validattion rules for LoginCommand, using FluentValidation library.x
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}