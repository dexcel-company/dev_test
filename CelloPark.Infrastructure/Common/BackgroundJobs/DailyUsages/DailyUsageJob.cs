using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Extensions;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Extensions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.CalculationExceptions.Enums;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Abstractions;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers.Abstractions;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers.Abstractions;
using CelloPark.Infrastructure.Common.Contexts;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages;

internal sealed class DailyUsageJob : IJob
{
    public const string Key = "DailyUsageJob";

    public DailyUsageJob(
        IExtractionWorker extractionWorker,
        ICalculationWorker calculationWorker,
        ISnapshotWorker snapshotWorker,
        TimeProvider timeProvider)
    {
        _extractionWorker = extractionWorker;
        _calculationWorker = calculationWorker;
        _snapshotWorker = snapshotWorker;
        _timeProvider = timeProvider;
    }

    private readonly IExtractionWorker _extractionWorker;
    private readonly ICalculationWorker _calculationWorker;
    private readonly ISnapshotWorker _snapshotWorker;
    private readonly TimeProvider _timeProvider;

    public async Task Execute(IJobExecutionContext context)
    {
        if (DailyUsageJobMonitor.IsRunning)
        {
            Console.WriteLine("Daily usage job already running.");

            return;
        }

        DailyUsageJobMonitor.Lock();
        Console.WriteLine($"Daily job started at {_timeProvider.GetUtcNow()} UTC.");

        using DailyUsageContext dailyUsageContext = new();

        await ExecuteWorkersAsync(dailyUsageContext, context.CancellationToken);

        DailyUsageJobMonitor.Unlock();
        Console.WriteLine($"Daily job completed at {_timeProvider.GetUtcNow()} UTC.");
    }

    private async Task ExecuteWorkersAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        bool success;

        success = await ExecuteExtractionAsync(dailyUsageContext, cancellationToken);

        if (!success)
        {
            return;
        }

        success = await ExecuteCalculationAsync(dailyUsageContext, cancellationToken);

        if (!success)
        {
            return;
        }

        await ExecuteSnapshotAsync(dailyUsageContext, cancellationToken);
    }

    private async Task<bool> ExecuteExtractionAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        return await _extractionWorker.ExecuteAsync(dailyUsageContext, cancellationToken);
    }

    private async Task<bool> ExecuteCalculationAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        const decimal FreePlanPrice = 0.0m;

        try
        {
            DateTimeOffset utcNow = _timeProvider.GetUtcNow();
            DateTime yesterday = new(utcNow.Year, utcNow.Month, utcNow.Day - 1, 23, 59, 59);
            DateOnly date = DateOnly.FromDateTime(yesterday);

            await dailyUsageContext.DailyItemUsageSummaries
                .Where(summary => summary.Date == date)
                .ExecuteDeleteAsync(cancellationToken);

            await dailyUsageContext.DailyPlanUsageSummaries
                .Where(summary => summary.Date == date)
                .ExecuteDeleteAsync(cancellationToken);

            await dailyUsageContext.DailyPackageUsageSummaries
                .Where(summary => summary.Date == date)
                .ExecuteDeleteAsync(cancellationToken);

            await dailyUsageContext.CustomerBenefits
                .Where(customerBenefit => customerBenefit.EndDate < yesterday)
                .ExecuteUpdateAsync(customerBenefit => customerBenefit.SetProperty(property => property.Status, Status.Deleted), cancellationToken);

            List<CustomerCalculationDto> customers = await dailyUsageContext.Customers
                .Where(customer => customer.DailyCharges.Any() && customer.Plan.Plan.Price > FreePlanPrice)
                .AsCalculationDto()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            List<BenefitCalculationDto> benefits = await dailyUsageContext.Benefits
                .AsCalculationDto()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            if (customers.Count == 0)
            {
                return false;
            }

            await _calculationWorker.ExecuteAsync(customers, benefits, yesterday, cancellationToken);

            return true;
        }
        catch (Exception exception)
        {
            CalculationException calculationException = CalculationException.Create(
                $"Failed to execute 'Calculation worker'. Error: {exception.Message}",
                CalculationExceptionType.Internal,
                _timeProvider.GetUtcNow());

            await dailyUsageContext.AddAsync(calculationException, cancellationToken);
            await dailyUsageContext.SaveChangesAsync(cancellationToken);

            return false;
        }
    }

    private async Task<bool> ExecuteSnapshotAsync(
        DailyUsageContext dailyUsageContext, CancellationToken cancellationToken = default)
    {
        return await _snapshotWorker.ExecuteAsync(dailyUsageContext, cancellationToken);
    }
}
