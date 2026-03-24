using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Features.Items.Commands.Create.Abstractions;
using CelloPark.Application.Features.Items.Extensions;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.ContractTypes.Errors;
using CelloPark.Domain.Features.Items;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Items.Commands.Create;

internal sealed class CreateItemCommandHandler :
    ICreateItemCommandHandler
{
    public CreateItemCommandHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<IdResult>> HandleAsync(
        CreateItemCommand request, CancellationToken cancellationToken = default)
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
                .AnyAsync(item => item.ShadowId == request.Dto.ShadowId, cancellationToken);

            if (exists)
            {
                return Error.Conflict("Item.Identifier.AlreadyExists", "Item with the same identifier already exists.");
            }
        }

        exists = await _managementContext.Items
            .AnyAsync(item => item.Name == request.Dto.Name, cancellationToken);

        if (exists)
        {
            return Error.Conflict("Item.Name.AlreadyExists", "Item with the same name already exists.");
        }

        ErrorOr<Item> itemResult = request.Dto.ToModel();

        if (itemResult.IsError)
        {
            return itemResult.Errors;
        }

        await _managementContext.Items.AddAsync(itemResult.Value, cancellationToken);
        await _managementContext.SaveChangesAsync(cancellationToken);

        return new IdResult(itemResult.Value.Id);
    }
}
