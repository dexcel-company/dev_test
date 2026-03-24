using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllBenefitCouponsQueryHandler
{
    Task<Page<BenefitCouponPageDto>> HandleAsync(
        GetAllBenefitCouponsQuery request, CancellationToken cancellationToken = default);
}
