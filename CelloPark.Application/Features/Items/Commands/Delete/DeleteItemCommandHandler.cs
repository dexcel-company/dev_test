using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Items.Commands.Delete.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Items.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Items.Commands.Delete;

internal sealed class DeleteItemCommandHandler :
    IDeleteItemCommandHandler
{
    public DeleteItemCommandHandler(IManagementContext managementContext)
    {
        _managementContext = managementContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        DeleteItemCommand request, CancellationToken cancellationToken = default)
    {
        Item? item = await _managementContext.Items
            .FirstOrDefaultAsync(x => x.Id == request.ItemId, cancellationToken);

        if (item is null)
        {
            return ItemErrors.NotFound;
        }

        bool isUsed = await _managementContext.Benefits
            .AnyAsync(item => item.PaymentCategories.Any(paymentCategory => paymentCategory.ItemId == request.ItemId), cancellationToken);

        if (isUsed)
        {
            return Error.Conflict("Item.InUse", "Item currently in use and cannot be deleted.");
        }

        item.MarkAsDeleted();

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
