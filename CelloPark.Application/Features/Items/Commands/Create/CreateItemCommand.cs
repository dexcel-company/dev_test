using CelloPark.Application.Features.Items.Dtos;

namespace CelloPark.Application.Features.Items.Commands.Create;

public sealed class CreateItemCommand
{
    public CreateItemCommand(ItemCreateDto dto)
    {
        Dto = dto;
    }

    public ItemCreateDto Dto { get; }
}
