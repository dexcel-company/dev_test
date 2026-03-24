using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Domain.Features.Benefits.Enums;

namespace CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll.Abstractions;

[SingletonHandler]
public interface IGetAllBenefitCouponTypesQueryHandler
{
    Page<CouponType> Handle(GetAllBenefitCouponTypesQuery request);
}
