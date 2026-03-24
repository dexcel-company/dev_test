using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Features.ContractTypes.Queries.GetAll;
using CelloPark.Application.Features.ContractTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Common.Enums.ContractTypes;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.ContractTypes.Endpoints;

public static class ContractTypeEndpoints
{
    public static void AddContractTypeEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Contract Types");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/contract-types")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllContractTypes)
            .Produces<Page<ContractType>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllContractTypes))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all contract types.",
            });
    }

    private static IResult GetAllContractTypes(
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllContractTypesQueryHandler handler)
    {
        GetAllContractTypesQuery request = new(paginationCriteria);
        Page<ContractType> result = handler.Handle(request);

        return Results.Ok(result);
    }
}
