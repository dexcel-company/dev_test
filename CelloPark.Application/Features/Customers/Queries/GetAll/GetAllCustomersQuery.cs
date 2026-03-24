using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Customers.Dtos;

namespace CelloPark.Application.Features.Customers.Queries.GetAll;

public sealed class GetAllCustomersQuery
{
    public GetAllCustomersQuery(
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        CustomerFilteringQuery filteringCriteria)
    {
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public CustomerFilteringQuery FilteringCriteria { get; }
}
