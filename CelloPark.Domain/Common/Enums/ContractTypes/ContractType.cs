using CelloPark.Domain.Common.Enums.Abstractions;

namespace CelloPark.Domain.Common.Enums.ContractTypes;

public sealed class ContractType :
    Enumeration<ContractType>
{
    public static readonly ContractType None = new(0, "None");
    public static readonly ContractType Private = new(1, "Private");
    public static readonly ContractType Business = new(2, "Business");

    private ContractType(byte key, string value) :
        base(key, value)
    { }
}
