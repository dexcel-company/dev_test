using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCredits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Customers.Dtos;

public sealed class CustomerGetDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required ContractType ContractType { get; init; }
    public required CustomerPlanGetDto? CustomerPlan { get; init; }
    public required IReadOnlyCollection<CustomerCarPageDto> CustomerCars { get; init; }
    public required IReadOnlyCollection<CustomerBenefitPageDto> CustomerBenefits { get; init; }
    public required IReadOnlyCollection<CustomerCreditPageDto> CustomerCredits { get; init; }
    public required DateTime? CreatedAt { get; init; }
}
