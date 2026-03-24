namespace CelloPark.Application.Features.Customers.Entities.CustomerDailyCharges.Dtos;

public sealed class CustomerDailyChargeCalculationDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid CustomerCarId { get; init; }
    public Guid ItemId { get; init; }
    public int Count { get; init; }
    public decimal Price { get; init; }
}
