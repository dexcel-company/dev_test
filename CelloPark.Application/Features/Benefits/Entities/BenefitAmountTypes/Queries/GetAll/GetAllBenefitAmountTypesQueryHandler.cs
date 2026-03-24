using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll;

public sealed class GetAllBenefitAmountTypesQueryHandler :
    IGetAllBenefitAmountTypesQueryHandler
{
    public Page<AmountType> Handle(GetAllBenefitAmountTypesQuery request)
    {
        Page<AmountType> amountTypePage = AmountType.Elements
            .ApplyPagination(request.PaginationCriteria);

        return amountTypePage;
    }
}
