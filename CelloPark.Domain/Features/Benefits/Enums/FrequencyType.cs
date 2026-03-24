using CelloPark.Domain.Common.Enums.Abstractions;

namespace CelloPark.Domain.Features.Benefits.Enums;

public sealed class FrequencyType :
    Enumeration<FrequencyType>
{
    public static readonly FrequencyType None = new(0, "None");
    public static readonly FrequencyType FirstTime = new(1, "First time");
    public static readonly FrequencyType EverySelectedTime = new(2, "Every selected time");
    public static readonly FrequencyType EveryTime = new(3, "Every time");
    public static readonly FrequencyType SeveralTime = new(4, "Several times");

    private FrequencyType(byte key, string value) :
        base(key, value)
    { }
}
