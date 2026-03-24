using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Plans.Dtos;

public sealed class PlanGetDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required ContractType ContractType { get; init; }
    public required CalculationType? CalculationType { get; init; }
    public required decimal Price { get; init; }
    public required bool HasVat { get; init; }
    public required DateOnly? StartDate { get; init; }
    public required DateOnly? EndDate { get; init; }
    public required IReadOnlyCollection<PackagePageDto> Packages { get; init; }
}
