using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Packets.Dtos;

public sealed class PackageGetDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required ContractType ContractType { get; init; }
    public required DateOnly? StartDate { get; init; }
    public required DateOnly? EndDate { get; init; }
}
