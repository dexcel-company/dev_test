using CelloPark.Application.Features.Items.Dtos;

namespace CelloPark.Application.Features.Items.Commands.Update;

public sealed class UpdateItemCommand
{
    public UpdateItemCommand(
        Guid itemId,
        ItemUpdateDto dto)
    {
        ItemId = itemId;
        Dto = dto;
    }

    public Guid ItemId { get; }
    public ItemUpdateDto Dto { get; }
}
