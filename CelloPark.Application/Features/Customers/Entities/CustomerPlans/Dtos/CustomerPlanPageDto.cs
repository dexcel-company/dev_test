using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;

public sealed class CustomerPlanPageDto
{
    public required Guid Id { get; init; }
    public required PlanLiteDto Plan { get; init; }
    public required int PackageCount { get; init; }
    public required decimal Price { get; init; }
    public required int Vat { get; init; }
}
