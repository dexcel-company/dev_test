using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll;
using CelloPark.Application.Features.Benefits.Entities.BenefitAmountTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Benefits.Endpoints;

public static class AmountTypeEndpoints
{
    public static void AddAmountTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Amount Types");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/amount-types")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllAmountTypes)
            .Produces<Page<CouponType>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllAmountTypes))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all amount types.",
            });
    }

    private static IResult GetAllAmountTypes(
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllBenefitAmountTypesQueryHandler handler)
    {
        GetAllBenefitAmountTypesQuery request = new(paginationCriteria);
        Page<AmountType> result = handler.Handle(request);

        return Results.Ok(result);
    }
}
