namespace CelloPark.Application.Features.Items.Dtos;

public sealed class ItemUpdateDto
{
    public long? ShadowId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public byte ContractType { get; init; }
    public decimal Price { get; init; }
    public bool HasVat { get; init; }
}
