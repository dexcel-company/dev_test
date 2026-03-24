using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Domain.Common.Enums.CalculationTypes;

namespace CelloPark.Application.Features.CalculationTypes.Queries.GetAll.Abstractions;

[SingletonHandler]
public interface IGetAllCalculationTypesQueryHandler
{
    Page<CalculationType> Handle(GetAllCalculationTypesQuery request);
}
