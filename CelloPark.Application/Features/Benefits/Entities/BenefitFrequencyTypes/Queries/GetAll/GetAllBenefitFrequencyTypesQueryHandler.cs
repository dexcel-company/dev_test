using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll;

internal sealed class GetAllBenefitFrequencyTypesQueryHandler :
    IGetAllBenefitFrequencyTypesQueryHandler
{
    public Page<FrequencyType> Handle(GetAllBenefitFrequencyTypesQuery request)
    {
        Page<FrequencyType> frequencyTypePage = FrequencyType.Elements
            .ApplyPagination(request.PaginationCriteria);

        return frequencyTypePage;
    }
}
