using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;

public sealed class CustomerBenefitPageDto
{
    public required Guid Id { get; init; }
    public required BenefitLiteDto Benefit { get; init; }
    public required CustomerCouponUsagePageDto? CouponUsage { get; init; }
    public required DateTime? StartDate { get; init; }
    public required DateTime? EndDate { get; init; }
}
