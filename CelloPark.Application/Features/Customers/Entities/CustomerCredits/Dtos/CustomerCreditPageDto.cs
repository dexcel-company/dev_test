using CelloPark.Application.Features.Items.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerCredits.Dtos;

public sealed class CustomerCreditPageDto
{
    public required Guid Id { get; init; }
    public required ItemLiteDto Item { get; init; }
    public required decimal Balance { get; init; }
}
