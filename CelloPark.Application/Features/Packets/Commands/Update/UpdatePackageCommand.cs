using CelloPark.Application.Features.Packets.Dtos;

namespace CelloPark.Application.Features.Packets.Commands.Update;

public sealed class UpdatePackageCommand
{
    public UpdatePackageCommand(
        Guid packageId,
        PackageUpdateDto dto)
    {
        PackageId = packageId;
        Dto = dto;
    }

    public Guid PackageId { get; }
    public PackageUpdateDto Dto { get; }
}
