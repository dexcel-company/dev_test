using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.Endpoints;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Benefits.Commands.ChangeStatus;
using CelloPark.Application.Features.Benefits.Commands.ChangeStatus.Abstractions;
using CelloPark.Application.Features.Benefits.Commands.Create;
using CelloPark.Application.Features.Benefits.Commands.Create.Abstractions;
using CelloPark.Application.Features.Benefits.Commands.Delete;
using CelloPark.Application.Features.Benefits.Commands.Delete.Abstractions;
using CelloPark.Application.Features.Benefits.Commands.Update;
using CelloPark.Application.Features.Benefits.Commands.Update.Abstractions;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Benefits.Queries.Export;
using CelloPark.Application.Features.Benefits.Queries.Export.Abstractions;
using CelloPark.Application.Features.Benefits.Queries.GetAll;
using CelloPark.Application.Features.Benefits.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Benefits.Queries.GetById;
using CelloPark.Application.Features.Benefits.Queries.GetById.Abstractions;
using CelloPark.Domain.Common.Results;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Benefits.Endpoints;

public static class BenefitEndpoints
{
    public static void AddBenefitEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Benefits");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/benefits")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllBenefits)
            .Produces<Page<BenefitPageDto>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllBenefits))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all benefits.",
            });

        groupV1
            .MapGet("/{benefitId:guid}", GetBenefitById)
            .Produces<BenefitGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetBenefitById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a benefit by id.",
            });

        groupV1
            .MapGet("/export", ExportBenefits)
            .Produces<FileStream>(StatusCodes.Status200OK)
            .WithName(nameof(ExportBenefits))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Export benefits to file.",
            });

        groupV1
            .MapPost("/", CreateBenefit)
            .Produces<BenefitGetDto>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .WithName(nameof(CreateBenefit))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new benefit.",
            });

        groupV1
            .MapPut("/{benefitId:guid}", UpdateBenefit)
            .Produces<BenefitGetDto>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(UpdateBenefit))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a benefit by id.",
            });

        groupV1
            .MapDelete("/{benefitId:guid}", DeleteBenefit)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(DeleteBenefit))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete a benefit by id.",
            });

        groupV1
            .MapGet("/coupons", GetAllBenefitCoupons)
            .Produces<Page<BenefitCouponPageDto>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllBenefitCoupons))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all benefit coupons.",
            });

        groupV1
            .MapPut("/statuses/{benefitId:guid}", ChangeBenefitStatus)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(ChangeBenefitStatus))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Change benefit status.",
            });
    }

    private static async Task<IResult> GetAllBenefits(
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] BenefitFilteringCriteria filteringCriteria,
        [FromServices] IGetAllBenefitsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllBenefitsQuery request = new(paginationCriteria, sortingCriteria, filteringCriteria);
        Page<BenefitPageDto> result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetBenefitById(
        [FromRoute] Guid benefitId,
        [FromServices] IGetBenefitByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetBenefitByIdQuery request = new(benefitId);
        ErrorOr<BenefitGetDto> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> ExportBenefits(
        [FromServices] IExportBenefitsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        ExportBenefitsQuery request = new();
        FileStream stream = await handler.HandleAsync(request, cancellationToken);

        return Results.Stream(stream, ContentType);
    }

    private static async Task<IResult> CreateBenefit(
        [FromBody] BenefitCreateDto dto,
        [FromServices] ICreateBenefitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        CreateBenefitCommand request = new(dto);
        ErrorOr<IdResult> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.CreatedAtRoute(nameof(GetBenefitById), new { BenefitId = value.Id }, value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> UpdateBenefit(
        [FromRoute] Guid benefitId,
        [FromBody] BenefitUpdateDto dto,
        [FromServices] IUpdateBenefitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        UpdateBenefitCommand request = new(benefitId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> DeleteBenefit(
        [FromRoute] Guid benefitId,
        [FromServices] IDeleteBenefitCommandHandler handler,
        CancellationToken cancellationToken)
    {
        DeleteBenefitCommand request = new(benefitId);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> GetAllBenefitCoupons(
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] BenefitCouponFilteringCriteria filteringCriteria,
        [FromServices] IGetAllBenefitCouponsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllBenefitCouponsQuery request = new(paginationCriteria, sortingCriteria, filteringCriteria);
        Page<BenefitCouponPageDto> result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> ChangeBenefitStatus(
        [FromRoute] Guid benefitId,
        [FromServices] IChangeBenefitStatusQueryHandler handler,
        CancellationToken cancellationToken)
    {
        ChangeBenefitStatusQuery request = new(benefitId);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }
}
