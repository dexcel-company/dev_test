using CelloPark.Domain.Features.Items;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerDailyCharges;

public sealed class CustomerDailyCharge
{
    public Guid Id { get; }
    public string CustomerId { get; } = null!;
    public string CarNumber { get; } = null!;
    public long ItemId { get; }
    public Item Item { get; } = null!;
    public int Count { get; }
    public decimal Price { get; }
}
