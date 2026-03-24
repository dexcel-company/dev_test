using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Customers.Dtos;

namespace CelloPark.Application.Features.Customers.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllCustomersQueryHandler
{
    Task<Page<CustomerPageDto>> HandleAsync(
        GetAllCustomersQuery request, CancellationToken cancellationToken = default);
}
