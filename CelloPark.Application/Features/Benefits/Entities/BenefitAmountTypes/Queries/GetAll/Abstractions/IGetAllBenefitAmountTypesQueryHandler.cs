using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll.Abstractions;

[SingletonHandler]
public interface IGetAllBenefitAmountTypesQueryHandler
{
    Page<AmountType> Handle(GetAllBenefitAmountTypesQuery request);
}
