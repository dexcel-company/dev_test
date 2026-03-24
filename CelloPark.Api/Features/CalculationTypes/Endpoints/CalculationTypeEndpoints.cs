using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.CalculationTypes.Queries.GetAll;
using CelloPark.Application.Features.CalculationTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Common.Enums.CalculationTypes;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.CalculationTypes.Endpoints;

public static class CalculationTypeEndpoints
{
    public static void AddCalculationTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Calculation Types");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/calculation-types")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllCalculationTypes)
            .Produces<Page<CalculationType>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllCalculationTypes))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all calculation types",
            });
    }

    private static IResult GetAllCalculationTypes(
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllCalculationTypesQueryHandler handler)
    {
        GetAllCalculationTypesQuery request = new(paginationCriteria);
        Page<CalculationType> result = handler.Handle(request);

        return Results.Ok(result);
    }
}
