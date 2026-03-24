using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.Abstractions;

public interface IDeleteDetailsOwner
{
    DeleteDetails DeleteDetails { get; }

    ErrorOr<None> AddDeleteDetails(DateTime? deletedAt, Guid? deletedBy);
}
