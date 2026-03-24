using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerCredits;

public sealed class CustomerCreditSnapshot
{
    private CustomerCreditSnapshot(
        string? description,
        decimal balance,
        string customerId)
    {
        Description = description;
        Balance = balance;
        CustomerId = customerId;
    }

    public string? Description { get; private set; }
    public decimal Balance { get; private set; }
    public string CustomerId { get; private set; } = null!;
    public DateOnly SnapshotDate { get; set; }

    public static ErrorOr<CustomerCreditSnapshot> Create(
        string? description,
        decimal balance,
        string customerId)
    {
        return new CustomerCreditSnapshot(
            description: description,
            balance: balance,
            customerId: customerId);
    }
}
