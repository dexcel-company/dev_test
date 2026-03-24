using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Packets.Commands.SetPriceForPlan.Abstractions;

[ScopedHandler]
public interface ISetPackagePriceForPlanCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        SetPackagePriceForPlanCommand request, CancellationToken cancellationToken = default);
}
