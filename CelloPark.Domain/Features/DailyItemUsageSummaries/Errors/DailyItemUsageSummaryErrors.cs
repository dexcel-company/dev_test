using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyItemUsageSummaries.Errors;

public static class DailyItemUsageSummaryErrors
{
    public static Error ItemIdIsInvalid => Error.Validation(
        code: "DailyItemUsageSummary.ItemId",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyItemUsageSummary.ItemId)));

    public static Error DateIsInvalid => Error.Validation(
        code: "DailyItemUsageSummary.Date",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyItemUsageSummary.Date)));

    public static Error GrossIsTooSmall => Error.Validation(
        code: "DailyItemUsageSummary.Gross",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyItemUsageSummary.Gross)));

    public static Error CostIsTooSmall => Error.Validation(
        code: "DailyItemUsageSummary.Cost",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyItemUsageSummary.Cost)));

    public static Error BenefitCostIsTooSmall => Error.Validation(
        code: "DailyItemUsageSummary.BenefitCost",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyItemUsageSummary.BenefitCost)));

    public static Error BenefitQuantityIsTooSmall => Error.Validation(
        code: "DailyItemUsageSummary.BenefitQuantity",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyItemUsageSummary.BenefitQuantity)));

    public static Error QuantityIsTooSmall => Error.Validation(
    code: "DailyItemUsageSummary.Quantity",
    description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyItemUsageSummary.Quantity)));

    public static Error NotFound => Error.NotFound(
        code: "DailyItemUsageSummary.NotFound",
        description: string.Format(ErrorDescriptions.NotFound, nameof(DailyItemUsageSummary)));
}
