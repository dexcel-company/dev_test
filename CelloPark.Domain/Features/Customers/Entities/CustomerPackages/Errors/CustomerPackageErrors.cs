using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerPackages.Errors;

public static class CustomerPackageErrors
{
    public static Error PriceIsTooSmall => Error.Validation(
        code: CustomerPackageErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(CustomerPackage.Price)));

    public static Error PriceIsTooBig => Error.Validation(
        code: CustomerPackageErrorCodes.Price,
        description: string.Format(ErrorDescriptions.TooBig, nameof(CustomerPackage.Price)));

    public static Error VatIsTooSmall => Error.Validation(
        code: CustomerPackageErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooSmall, nameof(CustomerPackage.Vat)));

    public static Error VatIsTooBig => Error.Validation(
        code: CustomerPackageErrorCodes.Vat,
        description: string.Format(ErrorDescriptions.TooBig, nameof(CustomerPackage.Vat)));

    public static Error NotFound => Error.NotFound(
        code: CustomerPackageErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(CustomerPackage)));
}
