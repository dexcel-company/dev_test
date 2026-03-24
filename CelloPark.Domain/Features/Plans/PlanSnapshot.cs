using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Domain.Features.Plans.Constants;
using CelloPark.Domain.Features.Plans.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Plans;

public sealed class PlanSnapshot
{
    private PlanSnapshot(
        Guid id,
        long shadowId,
        string name,
        ContractType contractType,
        CalculationType calculationType,
        decimal price,
        int vat)
    {
        Id = id;
        ShadowId = shadowId;
        Name = name;
        ContractType = contractType;
        CalculationType = calculationType;
        Price = price;
        Vat = vat;
    }

    public Guid Id { get; }
    public long ShadowId { get; private set; }
    public string Name { get; private set; } = null!;
    public ContractType ContractType { get; private set; } = null!;
    public CalculationType CalculationType { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Vat { get; private set; }
    public DateOnly SnapshotDate { get; set; }
    public IReadOnlyList<PlanPackageSnapshot> PlanPackages => _planPackages.AsReadOnly();

    private readonly List<PlanPackageSnapshot> _planPackages = [];

    public static ErrorOr<PlanSnapshot> Create(
        Guid id,
        long? shadowId,
        string name,
        ContractType? contractType,
        CalculationType? calculationType,
        decimal price,
        int vat)
    {
        ErrorOr<long> shadowIdResult = ValidateShadowId(shadowId);
        ErrorOr<string> nameResult = ValidateName(name);
        ErrorOr<ContractType> contractTypeResult = ValidateContractType(contractType);
        ErrorOr<CalculationType> calculationTypeResult = ValidateCalculationType(calculationType);
        ErrorOr<decimal> priceResult = ValidatePrice(price);
        ErrorOr<int> vatResult = ValidateVat(vat);

        List<Error> errors = ErrorProvider.Join(
            shadowIdResult,
            nameResult,
            contractTypeResult,
            calculationTypeResult,
            priceResult,
            vatResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new PlanSnapshot(
            id: id,
            shadowId: shadowIdResult.Value,
            name: nameResult.Value,
            contractType: contractTypeResult.Value,
            calculationType: calculationTypeResult.Value,
            price: priceResult.Value,
            vat: vatResult.Value);
    }

    public ErrorOr<None> UpdateName(string name)
    {
        ErrorOr<string> nameResult = ValidateName(name);

        if (nameResult.IsError)
        {
            return nameResult.FirstError;
        }

        Name = nameResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateContractType(ContractType? contractType)
    {
        ErrorOr<ContractType> contractTypeResult = ValidateContractType(contractType);

        if (contractTypeResult.IsError)
        {
            return contractTypeResult.FirstError;
        }

        ContractType = contractTypeResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateCalculationType(CalculationType? calculationType)
    {
        ErrorOr<CalculationType> calculationTypeResult = ValidateCalculationType(calculationType);

        if (calculationTypeResult.IsError)
        {
            return calculationTypeResult.FirstError;
        }

        CalculationType = calculationTypeResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdatePrice(decimal price)
    {
        ErrorOr<decimal> priceResult = ValidatePrice(price);

        if (priceResult.IsError)
        {
            return priceResult.FirstError;
        }

        Price = priceResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateVat(int vat)
    {
        ErrorOr<int> vatResult = ValidateVat(vat);

        if (vatResult.IsError)
        {
            return vatResult.FirstError;
        }

        Vat = vatResult.Value;

        return None.Value;
    }

    private static ErrorOr<long> ValidateShadowId(long? shadowId)
    {
        if (shadowId is null)
        {
            return long.MinValue;
        }

        if (shadowId < PlanSettings.ShadowIdMinValue)
        {
            return PlanErrors.ShadowIdIsTooSmall;
        }

        return shadowId.Value;
    }

    private static ErrorOr<string> ValidateName(string name)
    {
        if (name is null)
        {
            return PlanErrors.NameIsNull;
        }

        if (name.Length < PlanSettings.NameMinLength)
        {
            return PlanErrors.NameIsTooShort;
        }

        if (name.Length > PlanSettings.NameMaxLength)
        {
            return PlanErrors.NameIsTooLong;
        }

        return name;
    }

    private static ErrorOr<decimal> ValidatePrice(decimal price)
    {
        if (price < PlanSettings.PriceMinValue)
        {
            return PlanErrors.PriceIsTooSmall;
        }

        if (price > PlanSettings.PriceMaxValue)
        {
            return PlanErrors.PriceIsTooBig;
        }

        return price;
    }

    private static ErrorOr<ContractType> ValidateContractType(ContractType? contractType)
    {
        if (contractType is null)
        {
            return ContractType.None;
        }

        return contractType;
    }

    private static ErrorOr<CalculationType> ValidateCalculationType(CalculationType? calculationType)
    {
        if (calculationType is null)
        {
            return CalculationType.None;
        }

        return calculationType;
    }

    private static ErrorOr<int> ValidateVat(int vat)
    {
        if (vat < PlanSettings.VatMinValue)
        {
            return PlanErrors.VatIsTooSmall;
        }

        if (vat > PlanSettings.VatMaxValue)
        {
            return PlanErrors.VatIsTooBig;
        }

        return vat;
    }
}
