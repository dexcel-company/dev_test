using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Items.Dtos;

namespace CelloPark.Application.Features.Items.Queries.GetAll.Abstractions;

[ScopedHandler]
public interface IGetAllItemsQueryHandler
{
    Task<Page<ItemPageDto>> HandleAsync(
        GetAllItemsQuery request, CancellationToken cancellationToken = default);
}
