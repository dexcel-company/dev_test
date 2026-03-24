using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Errors;

public static class CustomerPlanErrors
{
    public static Error PriceIsTooSmall => Error.Validation(
        code: CustomerPlanErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(CustomerPlan.Price)));

    public static Error PriceIsTooBig => Error.Validation(
        code: CustomerPlanErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooBig, nameof(CustomerPlan.Price)));

    public static Error VatIsTooSmall => Error.Validation(
        code: CustomerPlanErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(CustomerPlan.Vat)));

    public static Error VatIsTooBig => Error.Validation(
        code: CustomerPlanErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooBig, nameof(CustomerPlan.Vat)));

    public static Error NotFound => Error.NotFound(
        code: CustomerPlanErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(CustomerPlan)));
}
