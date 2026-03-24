using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.DailyPackageUsageSummaries.Errors;

public static class DailyPackageUsageSummaryErrors
{
    public static Error PackageIdIsInvalid => Error.Validation(
        code: "DailyPackageUsageSummary.packageId",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyPackageUsageSummary.PackageId)));

    public static Error DateIsInvalid => Error.Validation(
        code: "DailyPackageUsageSummary.Date",
        description: string.Format(ErrorDescriptions.Invalid, nameof(DailyPackageUsageSummary.Date)));

    public static Error GrossIsTooSmall => Error.Validation(
        code: "DailyPackageUsageSummary.Gross",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPackageUsageSummary.Gross)));

    public static Error CostIsTooSmall => Error.Validation(
        code: "DailyPackageUsageSummary.Cost",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPackageUsageSummary.Cost)));

    public static Error BenefitCostIsTooSmall => Error.Validation(
        code: "DailyPackageUsageSummary.BenefitCost",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPackageUsageSummary.BenefitCost)));

    public static Error BenefitQuantityIsTooSmall => Error.Validation(
        code: "DailyPackageUsageSummary.BenefitQuantity",
        description: string.Format(ErrorDescriptions.TooSmall, nameof(DailyPackageUsageSummary.BenefitQuantity)));

    public static Error NotFound => Error.NotFound(
        code: "DailyPackageUsageSummary.NotFound",
        description: string.Format(ErrorDescriptions.NotFound, nameof(DailyPackageUsageSummary)));
}
