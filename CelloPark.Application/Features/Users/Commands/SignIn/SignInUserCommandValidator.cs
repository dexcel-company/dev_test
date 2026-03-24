using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Users;
using FluentValidation;

namespace CelloPark.Application.Features.Users.Commands.SignIn;

internal sealed class SignInUserCommandValidator :
    AbstractValidator<SignInUserCommand>
{
    public SignInUserCommandValidator()
    {
        // Email

        RuleFor(x => x.Dto.Email)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.Email)));

        // Password

        RuleFor(x => x.Dto.Password)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.Password)));
    }
}
