using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update.Abstractions;

[ScopedHandler]
public interface IUpdateCustomerPlanCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdateCustomerPlanCommand request, CancellationToken cancellationToken = default);
}
