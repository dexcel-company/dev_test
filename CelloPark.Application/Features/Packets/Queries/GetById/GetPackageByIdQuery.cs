namespace CelloPark.Application.Features.Packets.Queries.GetById;

public sealed class GetPackageByIdQuery
{
    public GetPackageByIdQuery(Guid packageId)
    {
        PackageId = packageId;
    }

    public Guid PackageId { get; }
}
