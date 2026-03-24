using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Packets.Dtos;

public sealed class PackagePageDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required IReadOnlyCollection<PlanLiteDto> RelatedPlans { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
