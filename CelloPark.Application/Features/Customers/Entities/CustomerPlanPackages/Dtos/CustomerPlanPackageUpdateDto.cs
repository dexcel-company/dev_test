namespace CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;

public sealed class CustomerPackageUpdateDto
{
    public decimal Price { get; init; }
    public bool HasVat { get; init; }
}
