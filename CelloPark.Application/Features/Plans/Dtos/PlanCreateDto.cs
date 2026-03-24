namespace CelloPark.Application.Features.Plans.Dtos;

public sealed class PlanCreateDto
{
    public long? ShadowId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public byte ContractType { get; init; }
    public byte CalculationType { get; init; }
    public decimal Price { get; init; }
    public bool HasVat { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}
