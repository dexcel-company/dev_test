namespace CelloPark.Application.Features.Packets.Dtos;

public sealed class PackagePlanCreateDto
{
    public decimal Price { get; init; }
    public bool HasVat { get; init; }
}
