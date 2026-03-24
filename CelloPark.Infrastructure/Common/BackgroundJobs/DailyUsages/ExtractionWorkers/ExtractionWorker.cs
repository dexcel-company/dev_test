using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.CalculationExceptions.Enums;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers.Abstractions;
using CelloPark.Infrastructure.Common.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers;

internal sealed class ExtractionWorker :
    IExtractionWorker
{
    private const string Sql = "EXECUTE [ExtractionJob] @dateFrom, @dateTo;";

    public ExtractionWorker(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    public async Task<bool> ExecuteAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        DateTimeOffset utcNow = _timeProvider.GetUtcNow();
        DateTime dateFrom = new(utcNow.Year, utcNow.Month, utcNow.Day - 1, 0, 0, 0);
        DateTime dateTo = new(utcNow.Year, utcNow.Month, utcNow.Day - 1, 23, 59, 59);

        SqlParameter[] parameters =
        [
            new SqlParameter("@dateFrom", dateFrom),
            new SqlParameter("@dateTo", dateTo)
        ];

        try
        {
            await dailyUsageContext.Database.ExecuteSqlRawAsync(Sql, parameters, cancellationToken);

            return true;
        }
        catch (Exception exception)
        {
            CalculationException calculationException = CalculationException.Create(
                $"Failed to execute [ExtractionJob] stored procedure. Error: {exception.Message}",
                CalculationExceptionType.Internal,
                _timeProvider.GetUtcNow());

            await dailyUsageContext.AddAsync(calculationException, cancellationToken);
            await dailyUsageContext.SaveChangesAsync(cancellationToken);

            return false;
        }
    }
}

