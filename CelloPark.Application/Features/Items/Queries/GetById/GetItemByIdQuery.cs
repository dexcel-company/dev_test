namespace CelloPark.Application.Features.Items.Queries.GetById;

public sealed class GetItemByIdQuery
{
    public GetItemByIdQuery(Guid itemId)
    {
        ItemId = itemId;
    }

    public Guid ItemId { get; }
}
