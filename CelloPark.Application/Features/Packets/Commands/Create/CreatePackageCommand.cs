using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Packets.Commands.Create;

public sealed class CreatePackageCommand
{
    public CreatePackageCommand(PackageCreateDto dto)
    {
        Dto = dto;
    }

    public PackageCreateDto Dto { get; }
}
