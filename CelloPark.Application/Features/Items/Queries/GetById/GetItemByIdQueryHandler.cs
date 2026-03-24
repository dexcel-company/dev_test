using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Application.Features.Items.Queries.GetById.Abstractions;
using CelloPark.Domain.Features.Items.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Items.Queries.GetById;

internal sealed class GetItemByIdQueryHandler :
    IGetItemByIdQueryHandler
{
    public GetItemByIdQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<ItemGetDto>> HandleAsync(
        GetItemByIdQuery request, CancellationToken cancellationToken = default)
    {
        ItemGetDto? itemGetDto = await _managementContext.Items
            .Where(item => item.Id == request.ItemId)
            .Select(item => new ItemGetDto
            {
                Id = item.Id,
                ShadowId = item.ShadowId,
                Name = item.Name,
                Description = item.Description,
                ContractType = item.ContractType,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (itemGetDto is null)
        {
            return ItemErrors.NotFound;
        }

        return itemGetDto;
    }
}
