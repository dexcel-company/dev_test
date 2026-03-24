using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage;

public sealed class UpdateCustomerPackageCommand
{
    public UpdateCustomerPackageCommand(
        Guid customerId,
        Guid customerPlanId,
        Guid CustomerPackageId,
        CustomerPackageUpdateDto dto)
    {
        CustomerId = customerId;
        CustomerPlanId = customerPlanId;
        CustomerPackageId = CustomerPackageId;
        Dto = dto;
    }

    public Guid CustomerId { get; }
    public Guid CustomerPlanId { get; }
    public Guid CustomerPackageId { get; }
    public CustomerPackageUpdateDto Dto { get; }
}
