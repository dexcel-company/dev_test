namespace CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Constants;

public static class CustomerPlanSettings
{
    public const decimal PriceMinValue = 0.0m;
    public const decimal PriceMaxValue = 10000.0m;

    public const int VatMinValue = 0;
    public const int VatMaxValue = 100;
    public const int VatDefaultValue = 17;
}
