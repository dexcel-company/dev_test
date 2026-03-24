using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Responses;
using ErrorOr;

namespace CelloPark.Application.Features.Benefits.Commands.Create.Abstractions;

[ScopedHandler]
public interface ICreateBenefitCommandHandler
{
    Task<ErrorOr<IdResult>> HandleAsync(
        CreateBenefitCommand request, CancellationToken cancellationToken = default);
}
