namespace CelloPark.Domain.Features.Plans.Constants;

public static class PlanSettings
{
    public const int ShadowIdMinValue = 1;

    public const int NameMinLength = 4;
    public const int NameMaxLength = 100;

    public const int DescriptionMaxLength = 500;

    public const decimal PriceMinValue = 0m;
    public const decimal PriceMaxValue = 10000m;

    public const int VatMinValue = 0;
    public const int VatMaxValue = 100;
    public const int VatDefaultValue = 17;
}
