using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.DailyItemUsageSummaries.Errors;
using CelloPark.Domain.Features.DailyPlanUsageSummaries.Constants;
using CelloPark.Domain.Features.DailyPlanUsageSummaries.Errors;
using CelloPark.Domain.Features.Plans;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPlanUsageSummaries;

public sealed class DailyPlanUsageSummary :
    IStatusOwner
{
    private DailyPlanUsageSummary() { }

    private DailyPlanUsageSummary(
        long planId,
        DateTime date,
        decimal gross,
        decimal cost,
        decimal benefitCost,
        int benefitQuantity,
        int quantity,
        int customerCount)
    {
        Id = Guid.NewGuid();
        PlanId = planId;
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
    public long PlanId { get; }
    public Plan Plan { get; } = null!;
    public DateTime Date { get; }
    public decimal Gross { get; private set; }
    public decimal Cost { get; private set; }
    public decimal BenefitCost { get; private set; }
    public int BenefitQuantity { get; private set; }
    public int Quantity { get; private set; }
    public int CustomerCount { get; private set; }
    public Status Status { get; private set; }

    public static ErrorOr<DailyPlanUsageSummary> Create(
        long planId,
        DateTime date,
        decimal gross,
        decimal cost,
        decimal benefitCost,
        int benefitQuantity,
        int quantity,
        int customerCount)
    {
        ErrorOr<long> planIdResult = ValidatePlanId(planId);
        ErrorOr<DateTime> dateResult = ValidateDate(date);
        ErrorOr<decimal> grossResult = ValidateGross(gross);
        ErrorOr<decimal> costResult = ValidateCost(cost);
        ErrorOr<decimal> benefitCostResult = ValidateBenefitCost(benefitCost);
        ErrorOr<int> benefitQuantityResult = ValidateBenefitQuantity(benefitQuantity);
        ErrorOr<int> QuantityResult = ValidateQuantity(quantity);
        ErrorOr<int> CustomerCountResult = ValidateQuantity(customerCount);

        List<Error> errors = ErrorProvider.Join(
            planIdResult,
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

        return new DailyPlanUsageSummary(
            planId: planIdResult.Value,
            date: dateResult.Value,
            gross: grossResult.Value,
            cost: costResult.Value,
            benefitCost: benefitCostResult.Value,
            benefitQuantity: benefitQuantityResult.Value,
            quantity: QuantityResult.Value,
            customerCount: CustomerCountResult.Value);
    }

    public void UpdateRercord(decimal gross, decimal cost)
    {
        Quantity++;
        BenefitQuantity += gross > cost ? 1 : 0;
        BenefitCost += gross - cost;
        Gross += gross;
        Cost += cost;
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

    private static ErrorOr<long> ValidatePlanId(long planId)
    {
        return planId;
    }

    private static ErrorOr<DateTime> ValidateDate(DateTime date)
    {
        if (date == default || date == DateTime.MinValue || date == DateTime.MaxValue)
        {
            return DailyPlanUsageSummaryErrors.DateIsInvalid;
        }

        return date;
    }

    private static ErrorOr<decimal> ValidateGross(decimal gross)
    {
        if (gross < DailyPlanUsageSummarySettings.GrossMinValue)
        {
            return DailyPlanUsageSummaryErrors.GrossIsTooSmall;
        }

        return gross;
    }

    private static ErrorOr<decimal> ValidateCost(decimal cost)
    {
        if (cost < DailyPlanUsageSummarySettings.CostMinValue)
        {
            return DailyPlanUsageSummaryErrors.CostIsTooSmall;
        }

        return cost;
    }

    private static ErrorOr<decimal> ValidateBenefitCost(decimal benefitCost)
    {
        if (benefitCost < DailyPlanUsageSummarySettings.BenefitCostMinValue)
        {
            return DailyPlanUsageSummaryErrors.BenefitCostIsTooSmall;
        }

        return benefitCost;
    }

    private static ErrorOr<int> ValidateBenefitQuantity(int benefitQuantity)
    {
        if (benefitQuantity < DailyPlanUsageSummarySettings.BenefitQuantity)
        {
            return DailyPlanUsageSummaryErrors.BenefitQuantityIsTooSmall;
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
