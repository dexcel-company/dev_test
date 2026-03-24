using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.Items.Queries.GetAll;

public sealed class GetAllItemsQuery
{
    public GetAllItemsQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
}
