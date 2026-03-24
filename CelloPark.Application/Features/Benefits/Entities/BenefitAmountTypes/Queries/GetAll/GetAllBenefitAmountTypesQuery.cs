using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll;

public sealed class GetAllBenefitAmountTypesQuery
{
    public GetAllBenefitAmountTypesQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }

}
