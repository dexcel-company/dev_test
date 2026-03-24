using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Plans.Dtos;

public sealed class PlanPageDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required decimal? Price { get; init; }
    public required ContractType ContractType { get; init; }
    public required CalculationType CalculationType { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
