using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create.Errors;
using CelloPark.Domain.Features.Users;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;

public sealed class CreateDetails
{
    public DateTime? CreatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public User? User { get; private set; }

    private CreateDetails(DateTime? createdAt, Guid? createdBy)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    public static ErrorOr<CreateDetails> Create(DateTime? createdAt, Guid? createdBy)
    {
        List<Error> errors = [];

        ErrorOr<DateTime?> createdAtResult = ValidateCreatedAt(createdAt);

        if (createdAtResult.IsError)
        {
            errors.Add(createdAtResult.FirstError);
        }

        ErrorOr<Guid?> createdByResult = ValidateCreatedBy(createdBy);

        if (createdByResult.IsError)
        {
            errors.Add(createdByResult.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new CreateDetails(createdAtResult.Value, createdByResult.Value);
    }

    private static ErrorOr<DateTime?> ValidateCreatedAt(DateTime? createdAt)
    {
        if (createdAt is null)
        {
            return createdAt;
        }

        if (createdAt.Value == default
            || createdAt.Value == DateTime.MinValue
            || createdAt.Value == DateTime.MaxValue
            || createdAt.Value > DateTime.UtcNow)
        {
            return CreationDetailErrors.CreatedAtIsInvalid;
        }

        return createdAt;
    }

    private static ErrorOr<Guid?> ValidateCreatedBy(Guid? createdBy)
    {
        if (createdBy is null)
        {
            return createdBy;
        }

        if (createdBy.Value == default)
        {
            return CreationDetailErrors.CreatedByIsInvalid;
        }

        return createdBy;
    }
}
