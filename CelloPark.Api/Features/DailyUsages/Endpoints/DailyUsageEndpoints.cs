using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate;
using CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate.Abstractions;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.Export;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.Export.Abstractions;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.DailyUsages.Endpoints;

public static class DailyUsageEndpoints
{
    public static void AddDashboardEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Daily Usages");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/daily-usages")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAll)
            .Produces<DailyUsageSummaryPageDto>(StatusCodes.Status200OK)
            .WithName(nameof(GetAll))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all daily usage summaries.",
            });

        groupV1
            .MapGet("/export", ExportDailyUsages)
            .Produces<FileStream>(StatusCodes.Status200OK)
            .WithName(nameof(ExportDailyUsages))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Export daily usage summaries to file.",
            });

        groupV1
            .MapPost("/", CalculateDailyUsages)
            .Produces(StatusCodes.Status204NoContent)
            .WithName(nameof(CalculateDailyUsages))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Calculate daily usages.",
            });
    }

    public static async Task<IResult> GetAll(
        [AsParameters] DailyUsageSummaryFilteringQuery filteringCriteria,
        [FromServices] IGetAllDailyUsageSummaryQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllDailyUsageSummaryQuery request = new(filteringCriteria);
        DailyUsageSummaryPageDto result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    public static async Task<IResult> ExportDailyUsages(
        [AsParameters] DailyUsageSummaryExportFilteringCriteria filteringCriteria,
        [FromServices] IExportDailyUsageSummariesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        ExportDailyUsageSummariesQuery request = new(filteringCriteria);
        FileStream stream = await handler.HandleAsync(request, cancellationToken);

        return Results.Stream(stream, ContentType);
    }

    public static async Task<IResult> CalculateDailyUsages(
        [FromBody] DailyUsageSummaryCalculateDto dto,
        [FromServices] ICalculateDailyUsageSummariesCommandHandler handler,
        CancellationToken cancellationToken)
    {
        CalculateDailyUsageSummariesCommand request = new(dto);
        await handler.HandleAsync(request, cancellationToken);

        return Results.NoContent();
    }
}
