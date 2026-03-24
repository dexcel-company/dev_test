namespace CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;

public sealed class CustomerPackageCalculationDto
{
    public Guid Id { get; init; }
    public Guid CustomerPlanId { get; init; }
    public Guid CustomerCarId { get; init; }
    public Guid PackageId { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public decimal Price { get; init; }
    public int Vat { get; init; }
}
