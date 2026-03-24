using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.Abstractions;

public interface IUpdateDetailsOwner
{
    UpdateDetails UpdateDetails { get; }

    ErrorOr<None> AddUpdateDetails(DateTime? updatedAt, Guid? updatedBy);
}
