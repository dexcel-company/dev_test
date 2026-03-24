namespace CelloPark.Application.Features.Packets.Dtos;

public sealed class PackageUpdateDto
{
    public long? ShadowId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public byte ContractType { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}
