using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Regexes;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Constants;
using FluentValidation;

namespace CelloPark.Application.Features.Users.Commands.SignUp;

internal sealed class SignUpUserCommandValidator :
    AbstractValidator<SignUpUserCommand>
{
    public SignUpUserCommandValidator()
    {
        // FirstName

        RuleFor(x => x.Dto.Firstname)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.FirstName)))
            .MinimumLength(UserSettings.FirstNameMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(User.FirstName)))
            .MaximumLength(UserSettings.FirstNameMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(User.FirstName)));

        // LastName

        RuleFor(x => x.Dto.Lastname)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.LastName)))
            .MinimumLength(UserSettings.LastNameMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(User.LastName)))
            .MaximumLength(UserSettings.LastNameMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(User.LastName)));

        // Email

        RuleFor(x => x.Dto.Email)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.Email)))
            .MinimumLength(UserSettings.EmailMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(User.Email)))
            .MaximumLength(UserSettings.EmailMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(User.Email)))
            .Matches(ValidationRegexes.EmailRegex)
            .WithMessage(string.Format(ErrorDescriptions.Invalid, nameof(User.Email)));

        // Password

        RuleFor(x => x.Dto.Password)
            .NotEmpty()
            .WithMessage(string.Format(ErrorDescriptions.Null, nameof(User.Password)))
            .MinimumLength(UserSettings.PasswordMinLength)
            .WithMessage(string.Format(ErrorDescriptions.TooShort, nameof(User.Password)))
            .MaximumLength(UserSettings.PasswordMaxLength)
            .WithMessage(string.Format(ErrorDescriptions.TooLong, nameof(User.Password)));
    }
}
