using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll.Abstractions;

[SingletonHandler]
public interface IGetAllBenefitFrequencyTypesQueryHandler
{
    Page<FrequencyType> Handle(GetAllBenefitFrequencyTypesQuery request);
}
