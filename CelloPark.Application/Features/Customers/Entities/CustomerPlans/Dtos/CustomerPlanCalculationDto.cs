using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;

public sealed class CustomerPlanCalculationDto
{
    public required Guid Id { get; init; }
    public required Guid PlanId { get; init; }
    public required DateOnly? StartDate { get; init; }
    public required DateOnly? EndDate { get; init; }
    public required decimal? Price { get; init; }
    public required int? Vat { get; init; }
    public required PlanCalculationDto Plan { get; init; }
    public required IReadOnlyCollection<CustomerPackageCalculationDto> PlanPackages { get; init; }
}
