using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPlanUsageSummaries.Errors;

public static class DailyPlanUsageSummaryErrors
{
    public static Error PlanIdIsInvalid => Error.Validation(
        code: "DailyPlanUsageSummary.PlanId",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyPlanUsageSummary.PlanId)));

    public static Error DateIsInvalid => Error.Validation(
        code: "DailyPlanUsageSummary.Date",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyPlanUsageSummary.Date)));

    public static Error GrossIsTooSmall => Error.Validation(
        code: "DailyPlanUsageSummary.Gross",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPlanUsageSummary.Gross)));

    public static Error CostIsTooSmall => Error.Validation(
    code: "DailyPlanUsageSummary.Cost",
    description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPlanUsageSummary.Cost)));

    public static Error BenefitCostIsTooSmall => Error.Validation(
        code: "DailyPlanUsageSummary.BenefitCost",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPlanUsageSummary.BenefitCost)));

    public static Error BenefitQuantityIsTooSmall => Error.Validation(
        code: "DailyPlanUsageSummary.BenefitQuantity",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPlanUsageSummary.BenefitQuantity)));

    public static Error NotFound => Error.NotFound(
        code: "DailyPlanUsageSummary.NotFound",
        description: string.Format(ErrorDescriptions.NotFound, nameof(DailyPlanUsageSummary)));
}
