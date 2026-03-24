using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Application.Features.Items.Queries.GetAll.Abstractions;

namespace CelloPark.Application.Features.Items.Queries.GetAll;

internal sealed class GetAllItemsQueryHandler :
    IGetAllItemsQueryHandler
{
    public GetAllItemsQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<Page<ItemPageDto>> HandleAsync(
        GetAllItemsQuery request, CancellationToken cancellationToken = default)
    {
        Page<ItemPageDto> itemPage = await _managementContext.Items
            .OrderBy(item => item.Id)
            .Select(item => new ItemPageDto
            {
                Id = item.Id,
                Name = item.Name,
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return itemPage;
    }
}
