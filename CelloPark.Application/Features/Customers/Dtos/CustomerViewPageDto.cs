using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Customers.Dtos;

public sealed class CustomerViewPageDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required ContractType ContractType { get; init; }
    public required string PlanName { get; init; }
    public required decimal PlanPrice { get; init; }
    public required int PackageCount { get; init; }
    public required int CarCount { get; init; }
    public required DateTime? CreatedAt { get; init; }
    public required UserAuditDto? CreatedBy { get; init; }
}
