using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.CalculationExceptions.Enums;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers.Abstractions;
using CelloPark.Infrastructure.Common.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers;

internal sealed class SnapshotWorker :
    ISnapshotWorker
{
    private const string Sql = "EXECUTE [SnapshotJob] @snapshotDate;";

    public SnapshotWorker(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    public async Task<bool> ExecuteAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        DateTimeOffset utcNow = _timeProvider.GetUtcNow();
        DateOnly snapshotDate = new(utcNow.Year, utcNow.Month, utcNow.Day - 1);
        SqlParameter parameter = new("@snapshotDate", snapshotDate);

        try
        {
            await dailyUsageContext.Database.ExecuteSqlRawAsync(Sql, parameter);

            return true;
        }
        catch (Exception exception)
        {
            CalculationException calculationException = CalculationException.Create(
                $"Failed to execute [SnapshotJob] stored procedure. Error: {exception.Message}",
                CalculationExceptionType.Internal,
                _timeProvider.GetUtcNow());

            await dailyUsageContext.AddAsync(calculationException, cancellationToken);
            await dailyUsageContext.SaveChangesAsync(cancellationToken);

            return false;
        }
    }
}
