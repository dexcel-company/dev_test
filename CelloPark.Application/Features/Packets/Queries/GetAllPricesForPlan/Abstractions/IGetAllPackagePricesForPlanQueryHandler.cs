using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Packets.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Queries.GetAllPricesForPlan.Abstractions;

[ScopedHandler]
public interface IGetAllPackagePricesForPlanQueryHandler
{
    Task<ErrorOr<Page<PackagePlanPageDto>>> HandleAsync(
        GetAllPackagePricesForPlanQuery request, CancellationToken cancellationToken = default);
}
