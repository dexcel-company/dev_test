using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Common.Enums.ContractTypes.Errors;

public static class ContractTypeErrors
{
    public static Error NotFound => Error.NotFound(
        code: ContractTypeErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(ContractType)));
}
