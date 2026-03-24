using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;

public sealed class CustomerPlanGetDto
{
    public required Guid Id { get; init; }
    public required PlanLiteDto Plan { get; init; }
    public required IReadOnlyCollection<CustomerPackagePageDto> CustomerPackages { get; init; }
    public required decimal Price { get; init; }
    public required int Vat { get; init; }
}
