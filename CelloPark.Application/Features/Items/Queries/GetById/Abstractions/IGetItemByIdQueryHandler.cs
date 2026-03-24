using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Features.Items.Dtos;
using ErrorOr;

namespace CelloPark.Application.Features.Items.Queries.GetById.Abstractions;

[ScopedHandler]
public interface IGetItemByIdQueryHandler
{
    Task<ErrorOr<ItemGetDto>> HandleAsync(
        GetItemByIdQuery request, CancellationToken cancellationToken = default);
}
