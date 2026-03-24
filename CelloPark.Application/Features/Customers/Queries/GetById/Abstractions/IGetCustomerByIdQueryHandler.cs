using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Customers.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetCustomerByIdQueryHandler
{
    Task<ErrorOr<CustomerGetDto>> HandleAsync(
        GetCustomerByIdQuery request, CancellationToken cancellationToken = default);
}
