using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Features.Packets.Commands.Create.Abstractions;
using CelloPark.Application.Features.Packets.Extensions;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.ContractTypes.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Packets.Commands.Create;

internal sealed class CreatePackageCommandHandler :
    ICreatePackageCommandHandler
{
    public CreatePackageCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<IdResult>> HandleAsync(
        CreatePackageCommand request, CancellationToken cancellationToken = default)
    {
        CreatePackageCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));
        }

        ErrorOr<None> requestResult = await ValidateRequestAsync(request, cancellationToken);

        if (requestResult.IsError)
        {
            return requestResult.FirstError;
        }

        ErrorOr<Package> packageResult = request.Dto.ToModel();

        if (packageResult.IsError)
        {
            return packageResult.Errors;
        }

        await _managementContext.Packages.AddAsync(packageResult.Value, cancellationToken);
        await _managementContext.SaveChangesAsync(cancellationToken);

        return new IdResult(packageResult.Value.Id);
    }

    private async Task<ErrorOr<None>> ValidateRequestAsync(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _managementContext.Packages
            .AnyAsync(package => package.Name == request.Dto.Name, cancellationToken);

        if (exists)
        {
            return PackageErrors.NameAlreadyExists;
        }

        ContractType? contractType = ContractType.FromKey(request.Dto.ContractType);

        if (contractType is null)
        {
            return ContractTypeErrors.NotFound;
        }

        if (request.Dto.ShadowId is not null)
        {
            exists = await _managementContext.Packages
                .AnyAsync(package => package.ShadowId == request.Dto.ShadowId, cancellationToken);

            if (exists)
            {
                return PackageErrors.ShadowIdAlreadyExists;
            }
        }

        return None.Value;
    }
}
