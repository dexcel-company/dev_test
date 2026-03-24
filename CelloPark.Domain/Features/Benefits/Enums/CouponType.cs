using CelloPark.Domain.Common.Enums.Abstractions;

namespace CelloPark.Domain.Features.Benefits.Enums;

public sealed class CouponType :
    Enumeration<CouponType>
{
    public static readonly CouponType None = new(0, "None");
    public static readonly CouponType OneTime = new(1, "One time");
    public static readonly CouponType MultiplyTime = new(2, "Multiply times");

    private CouponType(byte key, string value) :
        base(key, value)
    { }
}
