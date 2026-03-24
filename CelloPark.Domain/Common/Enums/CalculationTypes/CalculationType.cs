using CelloPark.Domain.Common.Enums.Abstractions;

namespace CelloPark.Domain.Common.Enums.CalculationTypes;

public sealed class CalculationType :
    Enumeration<CalculationType>
{
    public static readonly CalculationType None = new(0, "None");
    public static readonly CalculationType Fixed = new(1, "Fixed price");
    public static readonly CalculationType ByCars = new(2, "By number of cars");
    public static readonly CalculationType ByUsages = new(3, "By used cars");

    private CalculationType(byte key, string value) :
        base(key, value)
    { }
}