using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Plans.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllPlansQueryHandler
{
    Task<Page<PlanPageDto>> HandleAsync(
        GetAllPlansQuery request, CancellationToken cancellationToken = default);
}
