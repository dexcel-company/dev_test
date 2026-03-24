using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages.Constants;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages.Errors;
using CelloPark.Domain.Features.Packages;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerPackages;

public sealed class CustomerPackage
{
    public string CustomerId { get; } = null!;
    public string CarNumber { get; } = null!;
    public CustomerCar CustomerCar { get; } = null!;
    public Guid PackageId { get; }
    public Package Package { get; } = null!;
    public Status Status { get; private set; }
    public DateOnly? StartDate { get; }
    public DateOnly? EndDate { get; }
    public decimal Price { get; private set; }
    public int Vat { get; private set; }

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

    private static ErrorOr<decimal> ValidatePrice(decimal price)
    {
        if (price < CustomerPackageSettings.PriceMinValue)
        {
            return CustomerPackageErrors.PriceIsTooSmall;
        }

        if (price > CustomerPackageSettings.PriceMaxValue)
        {
            return CustomerPackageErrors.PriceIsTooBig;
        }

        return price;
    }

    private static ErrorOr<int> ValidateVat(int vat)
    {
        if (vat < CustomerPackageSettings.VatMinValue)
        {
            return CustomerPackageErrors.VatIsTooSmall;
        }

        if (vat > CustomerPackageSettings.VatMaxValue)
        {
            return CustomerPackageErrors.VatIsTooBig;
        }

        return vat;
    }
}
