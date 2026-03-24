using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Responses;
using ErrorOr;

namespace CelloPark.Application.Features.Items.Commands.Create.Abstractions;

[ScopedHandler]
public interface ICreateItemCommandHandler
{
    Task<ErrorOr<IdResult>> HandleAsync(
        CreateItemCommand request, CancellationToken cancellationToken = default);
}
