using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Benefits.Commands.ChangeStatus.Abstractions;

[ScopedHandler]
public interface IChangeBenefitStatusQueryHandler
{
    Task<ErrorOr<None>> HandleAsync(
        ChangeBenefitStatusQuery request, CancellationToken cancellationToken = default);
}
