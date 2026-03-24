using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.Items.Dtos;

public sealed class ItemGetDto
{
    public required Guid Id { get; init; }
    public required long ShadowId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required ContractType ContractType { get; init; }
}