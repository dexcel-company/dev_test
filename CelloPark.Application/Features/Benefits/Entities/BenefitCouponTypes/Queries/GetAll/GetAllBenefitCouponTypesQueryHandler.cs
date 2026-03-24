using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll;

internal sealed class GetAllBenefitCouponTypesQueryHandler :
    IGetAllBenefitCouponTypesQueryHandler
{
    public Page<CouponType> Handle(GetAllBenefitCouponTypesQuery request)
    {
        Page<CouponType> couponTypePage = CouponType.Elements
            .ApplyPagination(request.PaginationCriteria);

        return couponTypePage;
    }
}
