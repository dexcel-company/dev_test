using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.DailyItemUsageCalculations;
using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Domain.Features.DailyPackageUsageCalculations;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Domain.Features.DailyPlanCalculations;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using CelloPark.Infrastructure.Common.Environments.Constants;
using CelloPark.Infrastructure.Common.Providers;
using Microsoft.Data.SqlClient;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Converters;

internal static class SqlBulkCopyConverter
{
    public static SqlBulkCopy ConvertCustomerBenefits(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[CustomerBenefit]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.BenefitId), "BenefitId");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.CustomerId), "CustomerId");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.FrequencyCountLeft), "FrequencyCountLeft");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.LimitAmountLeft), "LimitAmountLeft");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.StartDate), "StartDate");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.EndDate), "EndDate");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.Status), "Status");
        sqlBulk.ColumnMappings.Add("ValidityFlag", "ValidityFlag");
        sqlBulk.ColumnMappings.Add(nameof(CustomerBenefit.Debit), "Debit");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertItemUsageSummaries(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyItemUsageSummary]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.ItemId), "ItemId");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Date), "Date");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Gross), "Gross");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.BenefitCost), "BenefitCost");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.BenefitQuantity), "BenefitQuantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Quantity), "Quantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.CustomerCount), "CustomerCount");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageSummary.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertPlanUsageSummaries(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyPlanUsageSummary]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.PlanId), "PlanId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Date), "Date");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Gross), "Gross");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.BenefitCost), "BenefitCost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.BenefitQuantity), "BenefitQuantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Quantity), "Quantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.CustomerCount), "CustomerCount");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageSummary.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertPackageUsageSummaries(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyPackageUsageSummary]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.PackageId), "PackageId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Date), "Date");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Gross), "Gross");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.BenefitCost), "BenefitCost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.BenefitQuantity), "BenefitQuantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Quantity), "Quantity");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.CustomerCount), "CustomerCount");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageSummary.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertItemUsageCalculations(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyItemUsageCalculations]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.CustomerId), "CustomerId");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.CustomerCarId), "CustomerCarId");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.BenefitId), "BenefitId");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyItemUsageCalculation.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertPlanUsageCalculations(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyPlanUsageCalculations]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.CustomerId), "CustomerId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.CustomerPlanId), "CustomerPlanId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.BenefitId), "BenefitId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.CarCount), "CarCount");
        sqlBulk.ColumnMappings.Add(nameof(DailyPlanUsageCalculation.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertPackageUsageCalculations(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[DailyPackageUsageCalculations]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.CustomerId), "CustomerId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.CustomerPackageId), "CustomerPackageId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.CustomerCarId), "CustomerCarId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.BenefitId), "BenefitId");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.Cost), "Cost");
        sqlBulk.ColumnMappings.Add(nameof(DailyPackageUsageCalculation.Status), "Status");

        return sqlBulk;
    }

    public static SqlBulkCopy ConvertCalculationExceptions(this SqlBulkCopy sqlBulk)
    {
        if (sqlBulk.ColumnMappings.Count > 0)
        {
            sqlBulk.ColumnMappings.Clear();
        }

        sqlBulk.DestinationTableName = $"[{DatabaseProvider.Schema}].[CalculationException]";
        sqlBulk.BulkCopyTimeout = int.Parse(Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!);

        sqlBulk.ColumnMappings.Add(nameof(CalculationException.Id), "Id");
        sqlBulk.ColumnMappings.Add(nameof(CalculationException.Type), "Type");
        sqlBulk.ColumnMappings.Add(nameof(CalculationException.Message), "Message");
        sqlBulk.ColumnMappings.Add(nameof(CalculationException.DateTime), "ExtractionDate");

        return sqlBulk;
    }
}
