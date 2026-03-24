using CelloPark.Domain.Common.Enums.Abstractions;

namespace CelloPark.Domain.Features.Benefits.Enums;

public sealed class AmountType :
    Enumeration<AmountType>
{
    public static readonly AmountType None = new(0, "None");
    public static readonly AmountType Fixed = new(1, "Fixed");
    public static readonly AmountType Percent = new(2, "Percent");

    private AmountType(byte key, string value) :
        base(key, value)
    { }
}
