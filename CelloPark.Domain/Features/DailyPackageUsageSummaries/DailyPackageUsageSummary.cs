using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.DailyItemUsageSummaries.Errors;
using CelloPark.Domain.Features.DailyPackageUsageSummaries.Constants;
using CelloPark.Domain.Features.DailyPackageUsageSummaries.Errors;
using CelloPark.Domain.Features.DailyPlanUsageSummaries.Constants;
using CelloPark.Domain.Features.Packages;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPackageUsageSummaries;

public sealed class DailyPackageUsageSummary :
    IStatusOwner
{
    private DailyPackageUsageSummary() { }

    private DailyPackageUsageSummary(
        long packageId,
        DateTime date,
        decimal gross,
        decimal cost,
        decimal benefitCost,
        int benefitQuantity,
        int quantity,
        int customerCount)
    {
        Id = Guid.NewGuid();
        PackageId = packageId;
        Date = date;
        Gross = gross;
        Cost = cost;
        BenefitCost = benefitCost;
        BenefitQuantity = benefitQuantity;
        Quantity = quantity;
        CustomerCount = customerCount;
        Status = Status.Active;
    }

    public Guid Id { get; }
    public long PackageId { get; }
    public Package Package { get; } = null!;
    public DateTime Date { get; }
    public decimal Gross { get; private set; }
    public decimal Cost { get; private set; }
    public decimal BenefitCost { get; private set; }
    public int BenefitQuantity { get; private set; }
    public int Quantity { get; private set; }
    public int CustomerCount { get; private set; }
    public Status Status { get; private set; }

    public static ErrorOr<DailyPackageUsageSummary> Create(
        long packageId,
        DateTime date,
        decimal gross,
        decimal cost,
        decimal benefitCost,
        int benefitQuantity,
        int quantity,
        int customerCount)
    {
        ErrorOr<long> packageIdResult = ValidatePackageId(packageId);
        ErrorOr<DateTime> dateResult = ValidateDate(date);
        ErrorOr<decimal> grossResult = ValidateGross(gross);
        ErrorOr<decimal> costResult = ValidateCost(cost);
        ErrorOr<decimal> benefitCostResult = ValidateBenefitCost(benefitCost);
        ErrorOr<int> benefitQuantityResult = ValidateBenefitQuantity(benefitQuantity);
        ErrorOr<int> QuantityResult = ValidateQuantity(quantity);
        ErrorOr<int> CustomerCountResult = ValidateQuantity(customerCount);

        List<Error> errors = ErrorProvider.Join(
            packageIdResult,
            dateResult,
            grossResult,
            costResult,
            benefitCostResult,
            benefitQuantityResult,
            QuantityResult,
            CustomerCountResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new DailyPackageUsageSummary(
            packageId: packageIdResult.Value,
            date: dateResult.Value,
            gross: grossResult.Value,
            cost: costResult.Value,
            benefitCost: benefitCostResult.Value,
            benefitQuantity: benefitQuantityResult.Value,
            quantity: QuantityResult.Value,
            customerCount: CustomerCountResult.Value);
    }

    public void UpdateRercord(decimal gross, decimal cost, bool unicUser)
    {
        Quantity++;
        BenefitQuantity += gross > cost ? 1 : 0;
        BenefitCost += gross - cost;
        Gross += gross;
        Cost += cost;
        if (unicUser)
            CustomerCount++;
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;
    }

    public void MarkAsActive()
    {
        Status = Status.Active;
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;
    }

    private static ErrorOr<long> ValidatePackageId(long packageId)
    {
        return packageId;
    }

    private static ErrorOr<DateTime> ValidateDate(DateTime date)
    {
        if (date == default || date == DateTime.MinValue || date == DateTime.MaxValue)
        {
            return DailyPackageUsageSummaryErrors.DateIsInvalid;
        }

        return date;
    }

    private static ErrorOr<decimal> ValidateGross(decimal gross)
    {
        if (gross < DailyPackageUsageSummarySettings.GrossMinValue)
        {
            return DailyPackageUsageSummaryErrors.GrossIsTooSmall;
        }

        return gross;
    }

    private static ErrorOr<decimal> ValidateCost(decimal cost)
    {
        if (cost < DailyPlanUsageSummarySettings.CostMinValue)
        {
            return DailyPackageUsageSummaryErrors.CostIsTooSmall;
        }

        return cost;
    }

    private static ErrorOr<decimal> ValidateBenefitCost(decimal benefitCost)
    {
        if (benefitCost < DailyPlanUsageSummarySettings.BenefitCostMinValue)
        {
            return DailyPackageUsageSummaryErrors.BenefitCostIsTooSmall;
        }

        return benefitCost;
    }

    private static ErrorOr<int> ValidateBenefitQuantity(int benefitQuantity)
    {
        if (benefitQuantity < DailyPlanUsageSummarySettings.BenefitQuantity)
        {
            return DailyPackageUsageSummaryErrors.BenefitQuantityIsTooSmall;
        }

        return benefitQuantity;
    }

    private static ErrorOr<int> ValidateQuantity(int Quantity)
    {
        if (Quantity < DailyPlanUsageSummarySettings.Quantity)
        {
            return DailyItemUsageSummaryErrors.QuantityIsTooSmall;
        }

        return Quantity;
    }
}
