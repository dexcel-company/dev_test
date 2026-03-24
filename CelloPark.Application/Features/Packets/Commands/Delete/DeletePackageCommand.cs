namespace CelloPark.Application.Features.Packets.Commands.Delete;

public sealed class DeletePackageCommand
{
    public DeletePackageCommand(Guid packageId)
    {
        PackageId = packageId;
    }

    public Guid PackageId { get; }
}
