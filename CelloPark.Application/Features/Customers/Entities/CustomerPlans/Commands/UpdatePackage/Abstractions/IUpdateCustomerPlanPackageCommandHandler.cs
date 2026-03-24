using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage.Abstractions;

[ScopedHandler]
public interface IUpdateCustomerPackageCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdateCustomerPackageCommand request, CancellationToken cancellationToken = default);
}
