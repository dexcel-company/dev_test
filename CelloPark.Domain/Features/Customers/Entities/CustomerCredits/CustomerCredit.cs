using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Items;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerCredits;

public sealed class CustomerCredit
{
    public string CustomerId { get; } = null!;
    public string? Description { get; }
    public decimal Balance { get; }
    public DateOnly? LastUpdateAt { get; }
    public Status Status { get; private set; }
}
