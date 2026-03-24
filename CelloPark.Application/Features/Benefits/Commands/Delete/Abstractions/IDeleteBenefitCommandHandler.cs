using CelloPark.Application.Common.Attributes;
using CelloPark.Domain.Common.Results;
using ErrorOr;

namespace CelloPark.Application.Features.Benefits.Commands.Delete.Abstractions;

[ScopedHandler]
public interface IDeleteBenefitCommandHandler
{
    Task<ErrorOr<None>> HandleAsync(
        DeleteBenefitCommand request, CancellationToken cancellationToken = default);
}
