using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using ErrorOr;

namespace CelloPark.Domain.Common.ValueObjects.Abstractions;

public interface ICreateDetailsOwner
{
    CreateDetails CreateDetails { get; }

    ErrorOr<None> AddCreateDetails(DateTime? createdAt, Guid? createdBy);
}
