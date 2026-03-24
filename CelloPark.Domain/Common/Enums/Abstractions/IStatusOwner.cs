using CelloPark.Domain.Common.Enums.Statuses;

namespace CelloPark.Domain.Common.Enums.Abstractions;

public interface IStatusOwner
{
    Status Status { get; }

    void MarkAsActive();
    void MarkAsInactive();
    void MarkAsDeleted();
}
