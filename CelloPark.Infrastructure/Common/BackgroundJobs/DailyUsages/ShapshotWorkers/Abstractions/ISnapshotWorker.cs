using CelloPark.Infrastructure.Common.Contexts;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers.Abstractions;

internal interface ISnapshotWorker
{
    Task<bool> ExecuteAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default);
}
