using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Features.Benefits.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CelloPark.Infrastructure.Common.Contexts.ValueConverters;

public static class DatabaseContextConverters
{
    public static readonly ValueConverter DateTimeConverter = new ValueConverter<DateTime, DateTime>(
        convertTo => convertTo,
        convertFrom => DateTime.SpecifyKind(convertFrom, DateTimeKind.Utc));

    public static readonly ValueConverter ContractTypeConverter = new ValueConverter<ContractType, byte>(
        convertTo => convertTo.Key,
        convertFrom => ContractType.FromKey(convertFrom) ?? ContractType.None);

    public static readonly ValueConverter CalculationTypeConverter = new ValueConverter<CalculationType, byte>(
        convertTo => convertTo.Key,
        convertFrom => CalculationType.FromKey(convertFrom) ?? CalculationType.None);

    public static readonly ValueConverter AmountTypeConverter = new ValueConverter<AmountType, byte>(
        convertTo => convertTo.Key,
        convertFrom => AmountType.FromKey(convertFrom) ?? AmountType.None);

    public static readonly ValueConverter FrequencyTypeConverter = new ValueConverter<FrequencyType, byte>(
        convertTo => convertTo.Key,
        convertFrom => FrequencyType.FromKey(convertFrom) ?? FrequencyType.None);

    public static readonly ValueConverter CouponTypeConverter = new ValueConverter<CouponType, byte>(
        convertTo => convertTo.Key,
        convertFrom => CouponType.FromKey(convertFrom) ?? CouponType.None);
}
