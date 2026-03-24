using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Plans.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Plans.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetPlanByIdQueryHandler
{
    Task<ErrorOr<PlanGetDto>> HandleAsync(
        GetPlanByIdQuery request, CancellationToken cancellationToken = default);
}
