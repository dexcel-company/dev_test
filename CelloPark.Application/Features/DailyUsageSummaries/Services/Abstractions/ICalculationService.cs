namespace CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;

public interface ICalculationService
{
    Task ExecuteAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
