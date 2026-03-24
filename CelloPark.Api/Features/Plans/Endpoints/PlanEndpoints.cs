using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.Endpoints;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Plans.Commands.Create;
using CelloPark.Application.Features.Plans.Commands.Create.Abstractions;
using CelloPark.Application.Features.Plans.Commands.Delete;
using CelloPark.Application.Features.Plans.Commands.Delete.Abstractions;
using CelloPark.Application.Features.Plans.Commands.Update;
using CelloPark.Application.Features.Plans.Commands.Update.Abstractions;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Plans.Queries.Export;
using CelloPark.Application.Features.Plans.Queries.Export.Abstractions;
using CelloPark.Application.Features.Plans.Queries.GetAll;
using CelloPark.Application.Features.Plans.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Plans.Queries.GetById;
using CelloPark.Application.Features.Plans.Queries.GetById.Abstractions;
using CelloPark.Domain.Common.Results;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Plans.Endpoints;

public static class PlanEndpoints
{
    public static void AddPlanEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Plans");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/plans")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllPlans)
            .Produces<Page<PlanPageDto>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllPlans))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all plans.",
            });

        groupV1
            .MapGet("/{planId:guid}", GetPlanById)
            .Produces<PlanGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetPlanById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a plan by id.",
            });

        groupV1
            .MapGet("/export", ExportPlans)
            .Produces<FileStream>(StatusCodes.Status200OK)
            .WithName(nameof(ExportPlans))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Export plans to file.",
            });

        groupV1
            .MapPost("/", CreatePlan)
            .Produces<CreatedResult>(StatusCodes.Status201Created)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(CreatePlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new plan.",
            });

        groupV1
            .MapPut("/{planId:guid}", UpdatePlan)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(UpdatePlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a plan by id.",
            });

        groupV1
            .MapDelete("/{planId:guid}", DeletePlan)
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(DeletePlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Delete a plan by id.",
            });
    }

    private static async Task<IResult> GetAllPlans(
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] PlanFilteringCriteria filteringCriteria,
        [FromServices] IGetAllPlansQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllPlansQuery request = new(paginationCriteria, sortingCriteria, filteringCriteria);
        Page<PlanPageDto> result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetPlanById(
        [FromRoute] Guid planId,
        [FromServices] IGetPlanByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetPlanByIdQuery request = new(planId);
        ErrorOr<PlanGetDto> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> ExportPlans(
        [FromServices] IExportPlansQueryHandler handler,
        CancellationToken cancellationToken)
    {
        const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        ExportPlansQuery request = new();
        FileStream stream = await handler.HandleAsync(request, cancellationToken);

        return Results.Stream(stream, ContentType);
    }

    private static async Task<IResult> CreatePlan(
        [FromBody] PlanCreateDto dto,
        [FromServices] ICreatePlanCommandHandler handler,
        CancellationToken cancellationToken)
    {
        CreatePlanCommand request = new(dto);
        ErrorOr<IdResult> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.CreatedAtRoute(nameof(GetPlanById), new { PlanId = value.Id }, value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> UpdatePlan(
        [FromRoute] Guid planId,
        [FromBody] PlanUpdateDto dto,
        [FromServices] IUpdatePlanCommandHandler handler,
        CancellationToken cancellationToken)
    {
        UpdatePlanCommand request = new(planId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> DeletePlan(
        [FromRoute] Guid planId,
        [FromServices] IDeletePlanCommandHandler handler,
        CancellationToken cancellationToken)
    {
        DeletePlanCommand request = new(planId);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }
}
