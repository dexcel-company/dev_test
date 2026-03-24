using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.DailyItemUsageCalculations;
using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Domain.Features.DailyPackageUsageCalculations;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Domain.Features.DailyPlanCalculations;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using System.Data;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Converters;

internal static class DataTableConverter
{
    public static DataTable ConvertCustomerBenefits(
        this DataTable dataTable, List<CustomerBenefit> customerBenefits)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(CustomerBenefit.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(CustomerBenefit.BenefitId), typeof(Guid));
        dataTable.Columns.Add(nameof(CustomerBenefit.CustomerId), typeof(Guid));
        dataTable.Columns.Add(nameof(CustomerBenefit.FrequencyCountLeft), typeof(int));
        dataTable.Columns.Add(nameof(CustomerBenefit.LimitAmountLeft), typeof(decimal));
        dataTable.Columns.Add(nameof(CustomerBenefit.StartDate), typeof(DateTime));
        dataTable.Columns.Add(nameof(CustomerBenefit.EndDate), typeof(DateTime));
        dataTable.Columns.Add(nameof(CustomerBenefit.Status), typeof(byte));
        dataTable.Columns.Add("ValidityFlag", typeof(int));
        dataTable.Columns.Add(nameof(CustomerBenefit.Debit), typeof(decimal));

        foreach (CustomerBenefit customerBenefit in customerBenefits)
        {
            dataTable.Rows.Add(
                customerBenefit.Id,
                customerBenefit.BenefitId,
                customerBenefit.CustomerId,
                customerBenefit.FrequencyCountLeft,
                customerBenefit.LimitAmountLeft,
                customerBenefit.StartDate,
                customerBenefit.EndDate,
                customerBenefit.Status,
                null,
                customerBenefit.Debit);
        }

        return dataTable;
    }

    public static DataTable ConvertItemUsageSummaries(
        this DataTable dataTable, Dictionary<Guid, DailyItemUsageSummary> itemUsageSummaries)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.ItemId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Date), typeof(DateOnly));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Gross), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.BenefitCost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.BenefitQuantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Quantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.CustomerCount), typeof(int));
        dataTable.Columns.Add(nameof(DailyItemUsageSummary.Status), typeof(byte));

        foreach (KeyValuePair<Guid, DailyItemUsageSummary> keyValuePair in itemUsageSummaries)
        {
            dataTable.Rows.Add(
                keyValuePair.Value.Id,
                keyValuePair.Value.ItemId,
                keyValuePair.Value.Date,
                keyValuePair.Value.Gross,
                keyValuePair.Value.Cost,
                keyValuePair.Value.BenefitCost,
                keyValuePair.Value.BenefitQuantity,
                keyValuePair.Value.Quantity,
                keyValuePair.Value.CustomerCount,
                keyValuePair.Value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertPlanUsageSummaries(
        this DataTable dataTable, Dictionary<Guid, DailyPlanUsageSummary> planUsageSummaries)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.PlanId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Date), typeof(DateOnly));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Gross), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.BenefitCost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.BenefitQuantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Quantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.CustomerCount), typeof(int));
        dataTable.Columns.Add(nameof(DailyPlanUsageSummary.Status), typeof(byte));

        foreach (KeyValuePair<Guid, DailyPlanUsageSummary> keyValuePair in planUsageSummaries)
        {
            dataTable.Rows.Add(
                keyValuePair.Value.Id,
                keyValuePair.Value.PlanId,
                keyValuePair.Value.Date,
                keyValuePair.Value.Gross,
                keyValuePair.Value.Cost,
                keyValuePair.Value.BenefitCost,
                keyValuePair.Value.BenefitQuantity,
                keyValuePair.Value.Quantity,
                keyValuePair.Value.CustomerCount,
                keyValuePair.Value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertPackageUsageSummaries(
        this DataTable dataTable, Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.PackageId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Date), typeof(DateOnly));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Gross), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.BenefitCost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.BenefitQuantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Quantity), typeof(int));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.CustomerCount), typeof(int));
        dataTable.Columns.Add(nameof(DailyPackageUsageSummary.Status), typeof(byte));

        foreach (KeyValuePair<Guid, DailyPackageUsageSummary> keyValuePair in packageUsageSummaries)
        {
            dataTable.Rows.Add(
                keyValuePair.Value.Id,
                keyValuePair.Value.PackageId,
                keyValuePair.Value.Date,
                keyValuePair.Value.Gross,
                keyValuePair.Value.Cost,
                keyValuePair.Value.BenefitCost,
                keyValuePair.Value.BenefitQuantity,
                keyValuePair.Value.Quantity,
                keyValuePair.Value.CustomerCount,
                keyValuePair.Value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertItemUsageCalculations(
        this DataTable dataTable, List<DailyItemUsageCalculation> itemUsageCalculations)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.CustomerId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.CustomerCarId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.BenefitId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyItemUsageCalculation.Status), typeof(byte));

        foreach (DailyItemUsageCalculation value in itemUsageCalculations)
        {
            dataTable.Rows.Add(
                value.Id,
                value.CustomerId,
                value.CustomerCarId,
                value.BenefitId,
                value.Cost,
                value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertPlanUsageCalculations(
        this DataTable dataTable, List<DailyPlanUsageCalculation> planUsageCalculations)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.CustomerId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.CustomerPlanId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.BenefitId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.CarCount), typeof(int));
        dataTable.Columns.Add(nameof(DailyPlanUsageCalculation.Status), typeof(byte));

        foreach (DailyPlanUsageCalculation value in planUsageCalculations)
        {
            dataTable.Rows.Add(
                value.Id,
                value.CustomerId,
                value.CustomerPlanId,
                value.BenefitId,
                value.Cost,
                value.CarCount,
                value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertPackageUsageCalculations(
        this DataTable dataTable, List<DailyPackageUsageCalculation> packageUsageSummaries)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.CustomerId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.CustomerPackageId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.CustomerCarId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.BenefitId), typeof(Guid));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.Cost), typeof(decimal));
        dataTable.Columns.Add(nameof(DailyPackageUsageCalculation.Status), typeof(byte));

        foreach (DailyPackageUsageCalculation value in packageUsageSummaries)
        {
            dataTable.Rows.Add(
                value.Id,
                value.CustomerId,
                value.CustomerPackageId,
                value.CustomerCarId,
                value.BenefitId,
                value.Cost,
                value.Status);
        }

        return dataTable;
    }

    public static DataTable ConvertCalculationExceptions(
        this DataTable dataTable, List<CalculationException> exceptions)
    {
        dataTable.ClearData();

        dataTable.Columns.Add(nameof(CalculationException.Id), typeof(Guid));
        dataTable.Columns.Add(nameof(CalculationException.Type), typeof(byte));
        dataTable.Columns.Add(nameof(CalculationException.Message), typeof(string));
        dataTable.Columns.Add(nameof(CalculationException.DateTime), typeof(DateTime));

        foreach (CalculationException exception in exceptions)
        {
            dataTable.Rows.Add(
                exception.Id,
                exception.Type,
                exception.Message,
                exception.DateTime);
        }

        return dataTable;
    }

    private static void ClearData(this DataTable dataTable)
    {
        if (dataTable.Columns.Count > 0)
        {
            dataTable.Columns.Clear();
        }

        if (dataTable.Rows.Count > 0)
        {
            dataTable.Rows.Clear();
        }
    }
}
