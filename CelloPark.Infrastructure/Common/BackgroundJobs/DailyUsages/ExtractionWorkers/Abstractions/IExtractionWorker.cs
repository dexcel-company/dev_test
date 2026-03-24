using CelloPark.Infrastructure.Common.Contexts;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers.Abstractions;

internal interface IExtractionWorker
{
    Task<bool> ExecuteAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default);
}
