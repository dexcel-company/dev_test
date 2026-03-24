using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.PlanPackages.Dto;

namespace CelloPark.Application.Features.Packets.Queries.GetAllPricesForPlan;

public sealed class GetAllPackagePricesForPlanQuery
{
    public GetAllPackagePricesForPlanQuery(
        Guid packageId,
        PaginationCriteria paginationCriteria,
        SortingCriteria sortingCriteria,
        PlanPackageFilteringCriteria filteringCriteria)
    {
        PackageId = packageId;
        PaginationCriteria = paginationCriteria;
        SortingCriteria = sortingCriteria;
        FilteringCriteria = filteringCriteria;
    }

    public Guid PackageId { get; }
    public PaginationCriteria PaginationCriteria { get; }
    public SortingCriteria SortingCriteria { get; }
    public PlanPackageFilteringCriteria FilteringCriteria { get; }
}
