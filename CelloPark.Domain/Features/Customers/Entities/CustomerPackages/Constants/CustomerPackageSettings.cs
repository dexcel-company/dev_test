namespace CelloPark.Domain.Features.Customers.Entities.CustomerPackages.Constants;

public static class CustomerPackageSettings
{
    public const decimal PriceMinValue = 0.0m;
    public const decimal PriceMaxValue = 10000.0m;

    public const int VatMinValue = 0;
    public const int VatMaxValue = 100;
    public const int VatDefaultValue = 17;
}
