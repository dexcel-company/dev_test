using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Benefits.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Benefits.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetBenefitByIdQueryHandler
{
    Task<ErrorOr<BenefitGetDto>> HandleAsync(
        GetBenefitByIdQuery request, CancellationToken cancellationToken = default);
}
