using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Items.Commands.Delete.Abstractions;

[ScopedHandler]
public interface IDeleteItemCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        DeleteItemCommand request, CancellationToken cancellationToken = default);
}
