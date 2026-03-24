using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll;

public sealed class GetAllBenefitCouponTypesQuery
{
    public GetAllBenefitCouponTypesQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
}
