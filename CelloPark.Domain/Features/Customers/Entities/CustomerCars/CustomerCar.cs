using CelloPark.Domain.Common.Enums.Statuses;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerCars;

public sealed class CustomerCar
{
    public string CustomerId { get; } = null!;
    public string Number { get; } = null!;
    public Status Status { get; private set; }
    public DateOnly? StartDate { get; }
    public DateOnly? EndDate { get; }
}
