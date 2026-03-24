namespace CelloPark.Application.Features.Items.Commands.Delete;

public sealed class DeleteItemCommand
{

    public DeleteItemCommand(Guid itemId)
    {
        ItemId = itemId;
    }

    public Guid ItemId { get; }
}
