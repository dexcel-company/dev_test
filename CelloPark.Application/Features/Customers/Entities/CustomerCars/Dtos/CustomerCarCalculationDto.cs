namespace CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;

public sealed class CustomerCarCalculationDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string Number { get; init; } = null!;
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}
