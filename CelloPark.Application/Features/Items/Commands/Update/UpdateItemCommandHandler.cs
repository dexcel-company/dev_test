using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Items.Commands.Update.Abstractions;
using CelloPark.Application.Features.Items.Extensions;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.ContractTypes.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Items.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Items.Commands.Update;

internal sealed class UpdateItemCommandHandler :
    IUpdateItemCommandHandler
{
    public UpdateItemCommandHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        UpdateItemCommand request, CancellationToken cancellationToken = default)
    {
        ContractType? contractType = ContractType.FromKey(request.Dto.ContractType);

        if (contractType is null)
        {
            return ContractTypeErrors.NotFound;
        }

        bool exists;

        if (request.Dto.ShadowId is not null)
        {
            exists = await _managementContext.Items
                .AnyAsync(item => item.ShadowId == request.Dto.ShadowId && item.Id != request.ItemId, cancellationToken);

            if (exists)
            {
                return Error.Conflict("Item.Identifier.AlreadyExists", "Item with the same identifier already exists.");
            }
        }

        exists = await _managementContext.Items
            .AnyAsync(item => item.Name == request.Dto.Name && item.Id != request.ItemId, cancellationToken);

        if (exists)
        {
            return Error.Conflict("Item.Name.AlreadyExists", "Item with the same name already exists.");
        }

        Item? item = await _managementContext.Items
            .FirstOrDefaultAsync(item => item.Id == request.ItemId, cancellationToken);

        if (item is null)
        {
            return ItemErrors.NotFound;
        }

        ErrorOr<Item> itemResult = item.Update(request.Dto);

        if (itemResult.IsError)
        {
            return itemResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
