using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Regexes;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Features.Roles;
using CelloPark.Domain.Features.Users.Constants;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions.Errors;
using CelloPark.Domain.Features.Users.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Users;

public sealed class User :
    ICreateDetailsOwner
{
    private User() { }

    private User(
        string? firstName,
        string? lastName,
        string? email,
        string? phoneNumber,
        string? jobTitle,
        string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        JobTitle = jobTitle;
        Password = password;
    }

    public Guid Id { get; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? JobTitle { get; private set; }
    public string Password { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public CreateDetails CreateDetails { get; private set; } = null!;
    public IReadOnlyList<RefreshSession> RefreshSessions => _refreshSessions.AsReadOnly();

    private readonly List<RefreshSession> _refreshSessions = [];

    public static ErrorOr<User> Create(
        string? firstName,
        string? lastName,
        string? email,
        string? phoneNumber,
        string? jobTitle,
        string password)
    {
        ErrorOr<string?> firstNameResult = ValidateFirstName(firstName);
        ErrorOr<string?> lastNameResult = ValidateLastName(lastName);
        ErrorOr<string?> emailResult = ValidateEmail(email);
        ErrorOr<string?> phoneNumberResult = ValidatePhoneNumber(phoneNumber);
        ErrorOr<string?> jobTitleResult = ValidateJobTitle(jobTitle);
        ErrorOr<string> passwordResult = ValidatePassword(password);

        List<Error> errors = ErrorProvider.Join(
            firstNameResult,
            lastNameResult,
            emailResult,
            phoneNumberResult,
            jobTitleResult,
            passwordResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(
            firstName: firstNameResult.Value,
            lastName: lastNameResult.Value,
            email: emailResult.Value,
            phoneNumber: phoneNumberResult.Value,
            jobTitle: jobTitleResult.Value,
            password: passwordResult.Value);
    }

    public ErrorOr<None> UpdateFirstName(string? firstName)
    {
        ErrorOr<string?> firstNameResult = ValidateFirstName(firstName);

        if (firstNameResult.IsError)
        {
            return firstNameResult.FirstError;
        }

        FirstName = firstNameResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateLastName(string? lastName)
    {
        ErrorOr<string?> lastNameResult = ValidateLastName(lastName);

        if (lastNameResult.IsError)
        {
            return lastNameResult.FirstError;
        }

        LastName = lastNameResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateEmail(string? email)
    {
        ErrorOr<string?> emailResult = ValidateEmail(email);

        if (emailResult.IsError)
        {
            return emailResult.FirstError;
        }

        Email = emailResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdatePhoneNumber(string? phoneNumber)
    {
        ErrorOr<string?> phoneNumberResult = ValidatePhoneNumber(phoneNumber);

        if (phoneNumberResult.IsError)
        {
            return phoneNumberResult.FirstError;
        }

        PhoneNumber = phoneNumberResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateJobTitle(string? jobTitle)
    {
        ErrorOr<string?> jobTitleResul = ValidateJobTitle(jobTitle);

        if (jobTitleResul.IsError)
        {
            return jobTitleResul.FirstError;
        }

        JobTitle = jobTitleResul.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdatePassword(string password)
    {
        ErrorOr<string> passwordResult = ValidatePassword(password);

        if (passwordResult.IsError)
        {
            return passwordResult.FirstError;
        }

        Password = passwordResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddCreateDetails(DateTime? createdAt, Guid? createdBy)
    {
        ErrorOr<CreateDetails> createDetailsResult = CreateDetails.Create(createdAt, createdBy);

        if (createDetailsResult.IsError)
        {
            return createDetailsResult.Errors;
        }

        CreateDetails = createDetailsResult.Value;

        return None.Value;
    }

    public ErrorOr<RefreshSession> AddRefreshSession(
        string refreshToken,
        string userAgent,
        string fingerprint,
        string ipAddress,
        DateTime createdAt)
    {
        ErrorOr<RefreshSession> refreshSessionResult = RefreshSession.Create(
            userId: Id,
            refreshToken: refreshToken,
            userAgent: userAgent,
            fingerprint: fingerprint,
            ipAddress: ipAddress,
            createdAt: createdAt);

        if (refreshSessionResult.IsError)
        {
            return refreshSessionResult.Errors;
        }

        _refreshSessions.Add(refreshSessionResult.Value);

        return refreshSessionResult.Value;
    }

    public ErrorOr<None> RemoveRefreshSession(RefreshSession refreshSession)
    {
        if (refreshSession is null)
        {
            return RefreshSessionErrors.NotFound;
        }

        _refreshSessions.Remove(refreshSession);

        return None.Value;
    }

    public ErrorOr<None> AddRole(Role role)
    {
        if (role is null)
        {
            return Error.Validation();
        }

        Role = role;

        return None.Value;
    }

    private static ErrorOr<string?> ValidateFirstName(string? firstName)
    {
        if (firstName is null)
        {
            return firstName;
        }

        if (firstName.Length < UserSettings.FirstNameMinLength)
        {
            return UserErrors.FirstNameIsTooShort;
        }

        if (firstName.Length > UserSettings.FirstNameMaxLength)
        {
            return UserErrors.FirstNameIsTooLong;
        }

        return firstName;
    }

    private static ErrorOr<string?> ValidateLastName(string? lastName)
    {
        if (lastName is null)
        {
            return lastName;
        }

        if (lastName.Length < UserSettings.LastNameMinLength)
        {
            return UserErrors.LastNameIsTooShort;
        }

        if (lastName.Length > UserSettings.LastNameMaxLength)
        {
            return UserErrors.LastNameIsTooLong;
        }

        return lastName;
    }

    private static ErrorOr<string?> ValidateEmail(string? email)
    {
        if (email is null)
        {
            return email;
        }

        if (email.Length < UserSettings.EmailMinLength)
        {
            return UserErrors.EmailIsTooShort;
        }

        if (email.Length > UserSettings.EmailMaxLength)
        {
            return UserErrors.EmailIsTooLong;
        }

        if (!ValidationRegexes.EmailRegex.IsMatch(email))
        {
            return UserErrors.EmailIsInvalid;
        }

        return email;
    }

    private static ErrorOr<string?> ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber is null)
        {
            return phoneNumber;
        }

        if (phoneNumber.Length < UserSettings.PhoneNumberMaxLength)
        {
            return UserErrors.PhoneNumberIsTooShort;
        }

        if (phoneNumber.Length > UserSettings.PhoneNumberMaxLength)
        {
            return UserErrors.PhoneNumberIsTooLong;
        }

        if (!ValidationRegexes.PhoneNumberRegex.IsMatch(phoneNumber))
        {
            return UserErrors.PhoneNumberIsInvalid;
        }

        return phoneNumber;
    }

    private static ErrorOr<string?> ValidateJobTitle(string? jobTitle)
    {
        if (jobTitle is null)
        {
            return jobTitle;
        }

        if (jobTitle.Length < UserSettings.JobTitleMinLength)
        {
            return UserErrors.JobTitleIsTooShort;
        }

        if (jobTitle.Length > UserSettings.JobTitleMaxLength)
        {
            return UserErrors.JobTitleIsTooLong;
        }

        return jobTitle;
    }

    private static ErrorOr<string> ValidatePassword(string password)
    {
        if (password is null)
        {
            return UserErrors.PasswordIsNull;
        }

        if (password.Length < UserSettings.PasswordMinLength)
        {
            return UserErrors.PasswordIsTooShort;
        }

        if (password.Length > UserSettings.PasswordMaxLength)
        {
            return UserErrors.PasswordIsTooLong;
        }

        return password;
    }
}
