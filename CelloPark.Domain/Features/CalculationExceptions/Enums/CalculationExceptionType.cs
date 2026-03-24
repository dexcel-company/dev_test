namespace CelloPark.Domain.Features.CalculationExceptions.Enums;

public enum CalculationExceptionType : byte
{
    Internal = 0,
    Customer = 1,
    Item = 2,
    Package = 3,
    Plan = 4,
    CustomerPlan = 5,
    DailyCharge = 6,
    CustomerCar = 7,
    CustomerView = 8,
    ContractType = 9,
}
