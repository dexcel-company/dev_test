using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Errors;

public static class CustomerErrors
{
    public static Error NotFound => Error.NotFound(
        code: CustomerErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(Customer)));
}
