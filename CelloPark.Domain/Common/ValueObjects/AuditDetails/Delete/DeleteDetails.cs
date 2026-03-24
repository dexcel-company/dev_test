using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete.Errors;
using CelloPark.Domain.Features.Users;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;

public sealed class DeleteDetails
{
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }
    public User? User { get; private set; }

    private DeleteDetails(DateTime? deletedAt, Guid? deletedBy)
    {
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }

    public static ErrorOr<DeleteDetails> Create(DateTime? deletedAt, Guid? deletedBy)
    {
        List<Error> errors = [];

        ErrorOr<DateTime?> deletedAtResult = ValidateDeletedAt(deletedAt);

        if (deletedAtResult.IsError)
        {
            errors.Add(deletedAtResult.FirstError);
        }

        ErrorOr<Guid?> deletedByResult = ValidateDeletedBy(deletedBy);

        if (deletedByResult.IsError)
        {
            errors.Add(deletedByResult.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new DeleteDetails(deletedAtResult.Value, deletedByResult.Value);
    }

    private static ErrorOr<DateTime?> ValidateDeletedAt(DateTime? deletedAt)
    {
        if (deletedAt is null)
        {
            return deletedAt;
        }

        if (deletedAt.Value == default
            || deletedAt.Value == DateTime.MinValue
            || deletedAt.Value == DateTime.MaxValue
            || deletedAt.Value > DateTime.UtcNow)
        {
            return DeleteDetailsErrors.DeletedAtIsInvalid;
        }

        return deletedAt;
    }

    private static ErrorOr<Guid?> ValidateDeletedBy(Guid? deletedBy)
    {
        if (deletedBy is null)
        {
            return deletedBy;
        }

        if (deletedBy.Value == default)
        {
            return DeleteDetailsErrors.DeletedByIsInvalid;
        }

        return deletedBy;
    }
}
