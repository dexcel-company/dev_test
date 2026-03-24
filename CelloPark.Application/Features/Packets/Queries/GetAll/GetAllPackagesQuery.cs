using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Packets.Queries.GetAll;

public sealed class GetAllPackagesQuery
{
    public GetAllPackagesQuery(
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        PackageFilteringCriteria filteringCriteria)
    {
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public PackageFilteringCriteria FilteringCriteria { get; }
}
