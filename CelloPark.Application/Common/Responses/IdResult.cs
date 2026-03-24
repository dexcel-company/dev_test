namespace CelloPark.Application.Common.Responses;

public sealed class IdResult
{
    public IdResult(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}
