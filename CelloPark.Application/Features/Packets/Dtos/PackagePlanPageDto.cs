using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Packets.Dtos;

public sealed class PackagePlanPageDto
{
    public required PlanPageDto Plan { get; init; }
    public required PackagePageDto Package { get; init; }
    public required decimal Price { get; init; }
    public required int Vat { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
