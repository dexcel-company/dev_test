using CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate.Abstractions;
using CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;

namespace CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate;

internal sealed class CalculateDailyUsageSummariesCommandHandler :
    ICalculateDailyUsageSummariesCommandHandler
{
    public CalculateDailyUsageSummariesCommandHandler(
        ICalculationService calculationService)
    {
        _calculationService = calculationService;
    }

    private readonly ICalculationService _calculationService;

    public async Task HandleAsync(
        CalculateDailyUsageSummariesCommand command, CancellationToken cancellationToken = default)
    {
        await _calculationService.ExecuteAsync(command.Dto.StartDate, command.Dto.EndDate, cancellationToken);
    }
}
