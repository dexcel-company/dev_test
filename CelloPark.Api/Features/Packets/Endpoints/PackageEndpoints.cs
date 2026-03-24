using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.Endpoints;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Packets.Commands.Create;
using CelloPark.Application.Features.Packets.Commands.Create.Abstractions;
using CelloPark.Application.Features.Packets.Commands.Delete;
using CelloPark.Application.Features.Packets.Commands.Delete.Abstractions;
using CelloPark.Application.Features.Packets.Commands.SetPriceForPlan;
using CelloPark.Application.Features.Packets.Commands.SetPriceForPlan.Abstractions;
using CelloPark.Application.Features.Packets.Commands.Update;
using CelloPark.Application.Features.Packets.Commands.Update.Abstractions;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Packets.Queries.Export;
using CelloPark.Application.Features.Packets.Queries.Export.Abstractions;
using CelloPark.Application.Features.Packets.Queries.GetAll;
using CelloPark.Application.Features.Packets.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Packets.Queries.GetAllPricesForPlan;
using CelloPark.Application.Features.Packets.Queries.GetAllPricesForPlan.Abstractions;
using CelloPark.Application.Features.Packets.Queries.GetById;
using CelloPark.Application.Features.Packets.Queries.GetById.Abstractions;
using CelloPark.Application.Features.PlanPackages.Dto;
using CelloPark.Domain.Common.Results;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Packets.Endpoints;

public static class PackageEndpoints
{
    public static void AddPackageEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Packages");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/packages")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllPackages)
            .Produces<Page<PackagePageDto>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllPackages))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all packages.",
            });

        groupV1
            .MapGet("/{packageId:guid}", GetPackageById)
            .Produces<PackageGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetPackageById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a package by id.",
            });

        groupV1
            .MapGet("/export", ExportPackages)
            .Produces<FileStream>(StatusCodes.Status200OK)
            .WithName(nameof(ExportPackages))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Export packages to file.",
            });

        groupV1
            .MapPost("/", CreatePackage)
            .Produces<PackageGetDto>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(CreatePackage))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new package.",
            });

        groupV1
            .MapPut("/{packageId:guid}", UpdatePackage)
            .Produces<PackageGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(UpdatePackage))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a package by id.",
            });

        groupV1
            .MapDelete("/{packageId:guid}", DeletePackage)
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(DeletePackage))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete a package by id.",
            });

        groupV1
            .MapGet("/{packageId:guid}/plans", GetAllPackagePricesForPlan)
            .Produces<Page<PackagePlanPageDto>>(StatusCodes.Status200OK)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetAllPackagePricesForPlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all package price for a plan.",
            });

        groupV1
            .MapPut("/{packageId:guid}/plans/{planId:guid}", SetPackagePriceForPlan)
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(SetPackagePriceForPlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Set a package price for a plan.",
            });
    }

    private static async Task<IResult> GetAllPackages(
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] PackageFilteringCriteria filteringCriteria,
        [FromServices] IGetAllPackagesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllPackagesQuery request = new(paginationCriteria, sortingCriteria, filteringCriteria);
        Page<PackagePageDto> result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetPackageById(
        [FromRoute] Guid packageId,
        [FromServices] IGetPackageByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetPackageByIdQuery request = new(packageId);
        ErrorOr<PackageGetDto> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> ExportPackages(
        [FromServices] IExportPackagesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        ExportPackagesQuery request = new();
        FileStream stream = await handler.HandleAsync(request, cancellationToken);

        return Results.Stream(stream, ContentType);
    }

    private static async Task<IResult> CreatePackage(
        [FromBody] PackageCreateDto dto,
        [FromServices] ICreatePackageCommandHandler handler,
        CancellationToken cancellationToken)
    {
        CreatePackageCommand request = new(dto);
        ErrorOr<IdResult> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.CreatedAtRoute(nameof(GetPackageById), new { PackageId = value.Id }, value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> UpdatePackage(
        [FromRoute] Guid packageId,
        [FromBody] PackageUpdateDto dto,
        [FromServices] IUpdatePackageCommandHandler handler,
        CancellationToken cancellationToken)
    {
        UpdatePackageCommand request = new(packageId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> DeletePackage(
        [FromRoute] Guid packageId,
        [FromServices] IDeletePackageCommandHandler handler,
        CancellationToken cancellationToken)
    {
        DeletePackageCommand request = new(packageId);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> GetAllPackagePricesForPlan(
        [FromRoute] Guid packageId,
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] PlanPackageFilteringCriteria filteringCriteria,
        [FromServices] IGetAllPackagePricesForPlanQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllPackagePricesForPlanQuery request = new(packageId, paginationCriteria, sortingCriteria, filteringCriteria);
        ErrorOr<Page<PackagePlanPageDto>> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> SetPackagePriceForPlan(
        [FromRoute] Guid packageId,
        [FromRoute] Guid planId,
        [FromBody] PackagePlanCreateDto dto,
        [FromServices] ISetPackagePriceForPlanCommandHandler handler,
        CancellationToken cancellationToken)
    {
        SetPackagePriceForPlanCommand request = new(packageId, planId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }
}
