using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Benefits.Commands.Update.Abstractions;

[ScopedHandler]
public interface IUpdateBenefitCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        UpdateBenefitCommand request, CancellationToken cancellationToken = default);
}
