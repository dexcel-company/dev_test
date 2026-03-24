using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.CalculationTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Common.Enums.CalculationTypes;

namespace CelloPark.Application.Features.CalculationTypes.Queries.GetAll;

internal sealed class GetAllCalculationTypesQueryHandler :
    IGetAllCalculationTypesQueryHandler
{
    public Page<CalculationType> Handle(GetAllCalculationTypesQuery request)
    {
        Page<CalculationType> calculationTypePage = CalculationType.Elements
            .ApplyPagination(request.PaginationCriteria);

        return calculationTypePage;
    }
}
