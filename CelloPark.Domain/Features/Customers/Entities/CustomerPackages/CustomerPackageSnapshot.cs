using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Packages;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerPackages;

public sealed class CustomerPackageSnapshot
{
    public string CustomerId { get; } = null!;
    public string CarNumber { get; } = null!;
    public CustomerCarSnapshot CustomerCar { get; } = null!;
    public long PackageId { get; }
    public PackageSnapshot Package { get; } = null!;
    public decimal Price { get; }
    public int Vat { get; }
    public DateOnly SnapshotDate { get; }
}
