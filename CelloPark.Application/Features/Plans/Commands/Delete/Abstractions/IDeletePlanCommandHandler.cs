using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Plans.Commands.Delete.Abstractions;

[ScopedHandler]
public interface IDeletePlanCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        DeletePlanCommand request, CancellationToken cancellationToken = default);
}
