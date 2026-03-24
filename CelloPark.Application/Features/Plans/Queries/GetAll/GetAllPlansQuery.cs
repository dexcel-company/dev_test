using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Plans.Queries.GetAll;

public sealed class GetAllPlansQuery
{
    public GetAllPlansQuery(
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        PlanFilteringCriteria filteringCriteria)
    {
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public PlanFilteringCriteria FilteringCriteria { get; }
}
