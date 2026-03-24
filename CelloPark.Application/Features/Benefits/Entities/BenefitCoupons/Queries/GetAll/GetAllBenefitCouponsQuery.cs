using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll;

public sealed class GetAllBenefitCouponsQuery
{
    public GetAllBenefitCouponsQuery(
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        BenefitCouponFilteringCriteria filteringCriteria)
    {
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public BenefitCouponFilteringCriteria FilteringCriteria { get; }
}
