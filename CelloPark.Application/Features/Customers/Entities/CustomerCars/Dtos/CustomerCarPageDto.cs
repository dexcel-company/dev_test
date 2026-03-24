namespace CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;

public sealed class CustomerCarPageDto
{
    public required Guid Id { get; init; }
    public required string Number { get; init; }
}
