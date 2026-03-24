using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Common.Enums.CalculationTypes.Errors;

public static class CalculationTypeErrors
{
    public static Error NotFound => Error.NotFound(
        code: "CalculationType.NotFound",
        description: string.Format(ErrorDescriptions.NotFound, nameof(CalculationType)));
}
