using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;
namespace CelloPark.Application.Features.Plans.Dtos;

public sealed class PlanCalculationDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required ContractType ContractType { get; init; } = null!;
    public required CalculationType CalculationType { get; init; } = null!;
    public required DateOnly? StartDate { get; init; }
    public required DateOnly? EndDate { get; init; }
    public required decimal Price { get; init; }
    public required int Vat { get; init; }
}
