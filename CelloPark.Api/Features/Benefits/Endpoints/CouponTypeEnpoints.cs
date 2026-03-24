using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll;
using CelloPark.Application.Features.Benefits.Entities.BenefitCouponTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Features.Benefits.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Benefits.Endpoints;

public static class CouponTypeEnpoints
{
    public static void AddCouponTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Coupon Types");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/coupon-types")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllCouponTypes)
            .Produces<Page<CouponType>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllCouponTypes))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all coupon types.",
            });
    }

    private static IResult GetAllCouponTypes(
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllBenefitCouponTypesQueryHandler handler)
    {
        GetAllBenefitCouponTypesQuery request = new(paginationCriteria);
        Page<CouponType> result = handler.Handle(request);

        return Results.Ok(result);
    }
}
