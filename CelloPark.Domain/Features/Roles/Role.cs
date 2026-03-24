using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Features.Roles.Constants;
using CelloPark.Domain.Features.Roles.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Roles;

public sealed class Role : ICreateDetailsOwner
{
    public Role() { }

    public Role(string name)
    {
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public CreateDetails CreateDetails { get; private set; } = null!;

    public static ErrorOr<Role> Create(string name)
    {
        ErrorOr<string> roleResult = ValidateName(name);

        List<Error> errors = ErrorProvider.Join(roleResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Role(name: roleResult.Value);
    }

    public ErrorOr<None> UpdateName(string name)
    {
        ErrorOr<string> nameResult = ValidateName(name);

        if (nameResult.IsError)
        {
            return nameResult.FirstError;
        }

        Name = nameResult.Value;

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

    private static ErrorOr<string> ValidateName(string name)
    {
        if (name is null)
        {
            return RoleErrors.Validation.Name.NullOrEmpty;
        }

        if (name.Length < RoleSettings.NameMixLength)
        {
            return RoleErrors.Validation.Name.TooShort;
        }

        if (name.Length > RoleSettings.NameMaxLength)
        {
            return RoleErrors.Validation.Name.TooLong;
        }

        return name;
    }
}
