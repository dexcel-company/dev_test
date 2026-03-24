using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update;

public sealed class UpdateCustomerPlanCommand
{
    public UpdateCustomerPlanCommand(
        Guid customerId,
        Guid customerPlanId,
        CustomerPlanUpdateDto dto)
    {
        CustomerId = customerId;
        CustomerPlanId = customerPlanId;
        Dto = dto;
    }

    public Guid CustomerId { get; }
    public Guid CustomerPlanId { get; }
    public CustomerPlanUpdateDto Dto { get; }
}
