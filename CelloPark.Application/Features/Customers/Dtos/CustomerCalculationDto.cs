using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerDailyCharges.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Customers.Dtos;

public sealed class CustomerCalculationDto
{
    public required Guid Id { get; init; }
    public required ContractType ContractType { get; init; } = null!;
    public required CustomerPlanCalculationDto Plan { get; init; } = null!;
    public required Guid CustomerPlanId { get; init; }
    public required IReadOnlyCollection<CustomerCarCalculationDto> Cars { get; init; }
    public required IReadOnlyCollection<CustomerBenefitCalculationDto> Benefits { get; init; }
    public required IReadOnlyCollection<CustomerDailyChargeCalculationDto> DailyCharges { get; init; }
}
