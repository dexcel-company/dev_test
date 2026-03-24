using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Plans.Commands.Update.Abstractions;

[ScopedHandler]
public interface IUpdatePlanCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdatePlanCommand request, CancellationToken cancellationToken = default);
}
