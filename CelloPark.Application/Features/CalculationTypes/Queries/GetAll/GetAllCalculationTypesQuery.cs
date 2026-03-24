using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.CalculationTypes.Queries.GetAll;

public sealed class GetAllCalculationTypesQuery
{
    public GetAllCalculationTypesQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
}
