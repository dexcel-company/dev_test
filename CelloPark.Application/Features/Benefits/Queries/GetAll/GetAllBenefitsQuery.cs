using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Benefits.Dtos;

namespace CelloPark.Application.Features.Benefits.Queries.GetAll;

public sealed class GetAllBenefitsQuery
{
    public GetAllBenefitsQuery(
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        BenefitFilteringCriteria filteringCriteria)
    {
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public BenefitFilteringCriteria FilteringCriteria { get; }
}
