using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Extensions;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll.Abstractions;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll;

internal sealed class GetAllBenefitCouponsQueryHandler :
    IGetAllBenefitCouponsQueryHandler
{
    private readonly IManagementContext _managementContext;

    public GetAllBenefitCouponsQueryHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    public async Task<Page<BenefitCouponPageDto>> HandleAsync(
        GetAllBenefitCouponsQuery request, CancellationToken cancellationToken = default)
    {
        Page<BenefitCouponPageDto> couponPage = await _managementContext.Benefits
            .SelectMany(benefit => benefit.Coupons)
            .ApplyFiltering(request.FilteringCriteria)
            .ApplySorting(request.SortingCriteria)
            .Select(coupon => new BenefitCouponPageDto
            {
                Id = coupon.Id,
                Coupon = coupon.Coupon,
                CouponType = coupon.CouponType,
                Duration = coupon.Duration,
                Status = coupon.Status.ToString(),
                IsUsed = _managementContext.CustomerCouponUsages
                    .Any(couponUsage => couponUsage.Coupon == coupon.Coupon),
                Benefit = new BenefitLiteDto
                {
                    Id = coupon.Benefit.Id,
                    Name = coupon.Benefit.Name,
                },
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return couponPage;
    }
}
