using CelloPark.Application.Common.Attributes;

namespace CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate.Abstractions;

[ScopedHandler]
public interface ICalculateDailyUsageSummariesCommandHandler
{
    Task HandleAsync(CalculateDailyUsageSummariesCommand command, CancellationToken cancellationToken = default);
}
