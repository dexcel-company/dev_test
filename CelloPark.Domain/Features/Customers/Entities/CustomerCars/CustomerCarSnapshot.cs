namespace CelloPark.Domain.Features.Customers.Entities.CustomerCars;

public sealed class CustomerCarSnapshot
{
    public string CustomerId { get; } = null!;
    public string Number { get; } = null!;
    public DateOnly SnapshotDate { get; }
}
