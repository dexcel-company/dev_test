using CelloPark.Application.Common.Pagination;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll;

public sealed class GetAllBenefitFrequencyTypesQuery
{
    public GetAllBenefitFrequencyTypesQuery(PaginationCriteria paginationCriteria)
    {
        PaginationCriteria = paginationCriteria;
    }

    public PaginationCriteria PaginationCriteria { get; }
}
