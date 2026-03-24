using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Packets.Commands.Update.Abstractions;
using CelloPark.Application.Features.Packets.Extensions;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.ContractTypes.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Packets.Commands.Update;

internal sealed class UpdatePackageCommandHandler :
    IUpdatePackageCommandHandler
{
    public UpdatePackageCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<None>> HandleAsync(
        UpdatePackageCommand request, CancellationToken cancellationToken = default)
    {
        UpdatePackageCommandValidator validator = new();
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

        Package? package = await _managementContext.Packages
            .FirstOrDefaultAsync(package => package.Id == request.PackageId, cancellationToken);

        if (package is null)
        {
            return PackageErrors.NotFound;
        }

        ErrorOr<Package> packageResult = package.Update(request.Dto);

        if (packageResult.IsError)
        {
            return packageResult.Errors;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }

    private async Task<ErrorOr<None>> ValidateRequestAsync(UpdatePackageCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _managementContext.Packages
            .AnyAsync(package => package.Name == request.Dto.Name && package.Id != request.PackageId, cancellationToken);

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
                .AnyAsync(package => package.ShadowId == request.Dto.ShadowId && package.Id != request.PackageId, cancellationToken);

            if (exists)
            {
                return PackageErrors.ShadowIdAlreadyExists;
            }
        }

        return None.Value;
    }
}
