using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.ContractTypes.Queries.GetAll;

public sealed class GetAllContractTypesQuery
{
    public GetAllContractTypesQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
}
