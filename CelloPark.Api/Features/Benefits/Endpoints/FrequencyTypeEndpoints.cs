using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll;
using CelloPark.Application.Features.Benefits.Entities.BenefitFrequencyTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Benefits.Endpoints;

public static class FrequencyTypeEndpoints
{
    public static void AddFrequencyTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Frequency Types");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/frequency-types")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllFrequencyTypes)
            .Produces<Page<CouponType>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllFrequencyTypes))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all frequency types.",
            });
    }

    private static IResult GetAllFrequencyTypes(
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllBenefitFrequencyTypesQueryHandler handler)
    {
        GetAllBenefitFrequencyTypesQuery request = new(paginationCriteria);
        Page<FrequencyType> result = handler.Handle(request);

        return Results.Ok(result);
    }
}
