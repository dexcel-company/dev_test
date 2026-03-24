using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Items.Commands.Update.Abstractions;

[ScopedHandler]
public interface IUpdateItemCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdateItemCommand request, CancellationToken cancellationToken = default);
}
