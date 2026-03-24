using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Responses;
using ErrorOr;

namespace CelloPark.Application.Features.Plans.Commands.Create.Abstractions;

[ScopedHandler]
public interface ICreatePlanCommandHandler
{
    Task<ErrorOr<IdResult>> HandleAsync(
        CreatePlanCommand request, CancellationToken cancellationToken = default);
}
