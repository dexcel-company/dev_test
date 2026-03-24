using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update.Errors;
using CelloPark.Domain.Features.Users;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;

public sealed class UpdateDetails
{
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public User? User { get; private set; }

    private UpdateDetails(DateTime? updatedAt, Guid? updatedBy)
    {
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
    }

    public static ErrorOr<UpdateDetails> Create(DateTime? updatedAt, Guid? updatedBy)
    {
        List<Error> errors = [];

        ErrorOr<DateTime?> updatedAtResult = ValidateUpdatedAt(updatedAt);

        if (updatedAtResult.IsError)
        {
            errors.Add(updatedAtResult.FirstError);
        }

        ErrorOr<Guid?> updatedByResult = ValidateUpdatedBy(updatedBy);

        if (updatedByResult.IsError)
        {
            errors.Add(updatedByResult.FirstError);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new UpdateDetails(updatedAtResult.Value, updatedByResult.Value);
    }

    private static ErrorOr<DateTime?> ValidateUpdatedAt(DateTime? updatedAt)
    {
        if (updatedAt is null)
        {
            return updatedAt;
        }

        if (updatedAt.Value == default
            || updatedAt.Value == DateTime.MinValue
            || updatedAt.Value == DateTime.MaxValue
            || updatedAt.Value > DateTime.UtcNow)
        {
            return UpdateDetailsErrors.UpdatedAtIsInvalid;
        }

        return updatedAt;
    }

    private static ErrorOr<Guid?> ValidateUpdatedBy(Guid? updatedBy)
    {
        if (updatedBy is null)
        {
            return updatedBy;
        }

        if (updatedBy.Value == default)
        {
            return UpdateDetailsErrors.UpdatedByIsInvalid;
        }

        return updatedBy;
    }
}
