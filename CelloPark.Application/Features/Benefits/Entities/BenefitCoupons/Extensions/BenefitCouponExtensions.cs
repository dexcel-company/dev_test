using CelloPark.Application.Common.Filtering.Extensions;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Extensions;

public static class BenefitCouponExtensions
{
    public static IQueryable<BenefitCoupon> ApplyFiltering(
        this IQueryable<BenefitCoupon> source, BenefitCouponFilteringCriteria filteringCriteria)
    {
        if (!string.IsNullOrWhiteSpace(filteringCriteria.Status)
            && Enum.TryParse(filteringCriteria.Status, true, out Status status))
        {
            if (Enum.IsDefined(status))
            {
                source = source
                    .IgnoreQueryFilters()
                    .Where(benefitCoupon => benefitCoupon.Status == status);
            }
        }

        if (!string.IsNullOrWhiteSpace(filteringCriteria.Search))
        {
            source = source
                .Where(benefitCoupon => EF.Functions.Like(benefitCoupon.Coupon, $"%{filteringCriteria.Search}%")
                    || EF.Functions.Like(benefitCoupon.Benefit.Name, $"%{filteringCriteria.Search}%"));
        }

        return source;
    }

    public static IOrderedQueryable<BenefitCoupon> ApplySorting(
        this IQueryable<BenefitCoupon> source, SortingCriteria sortingCriteria)
    {
        if (string.IsNullOrWhiteSpace(sortingCriteria.Sort))
        {
            return source.OrderBy(benefitCoupon => benefitCoupon.Id);
        }

        return sortingCriteria.Sort switch
        {
            _ when string.Equals(nameof(BenefitCoupon.Coupon), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefitCoupon => benefitCoupon.Coupon, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(BenefitCoupon.Duration), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefitCoupon => benefitCoupon.Duration, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(BenefitCoupon.Status), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefitCoupon => benefitCoupon.Status, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(BenefitCoupon.CouponType), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefitCoupon => benefitCoupon.CouponType, sortingCriteria.SortMethod),
            _ when string.Equals("BenefitName", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefitCoupon => benefitCoupon.Benefit.Name, sortingCriteria.SortMethod),
            _ =>
                source.OrderBy(benefitCoupon => benefitCoupon.Id),
        };
    }
}
