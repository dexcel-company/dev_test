using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Benefits.Dtos;

namespace CelloPark.Application.Features.Benefits.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllBenefitsQueryHandler
{
    Task<Page<BenefitPageDto>> HandleAsync(
        GetAllBenefitsQuery request, CancellationToken cancellationToken = default);
}
