using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;

public sealed class CustomerPackagePageDto
{
    public required Guid Id { get; init; }
    public required PackageLiteDto Package { get; init; }
    public required decimal Price { get; init; }
    public required int Vat { get; init; }
}
