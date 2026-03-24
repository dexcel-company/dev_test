using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.ExceptionHandlers;

namespace CelloPark.Api.Features.Healths.Endpoints;

public static class HealthEndpoints
{
    private static readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "Exceptions.txt");

    public static void AddheathEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Health");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:apiVersion}/health")
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", CheckHealthV1)
            .Produces<object>(StatusCodes.Status200OK)
            .WithName(nameof(CheckHealthV1))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Check the application health.",
            });

        groupV1
            .MapGet("/exceptions", GetExceptionsFile)
            .Produces(StatusCodes.Status200OK)
            .WithName(nameof(GetExceptionsFile))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all lines from exceptions file."
            });

        RouteGroupBuilder groupV2 = versionedApi
            .MapGroup("/api/v{version:apiVersion}/health")
            .HasApiVersion(new ApiVersion(2, 0));

        groupV2
            .MapGet("/", CheckHealthV2)
            .Produces<object>(StatusCodes.Status200OK)
            .WithName(nameof(CheckHealthV2))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Check the application health.",
            });
    }

    private static IResult CheckHealthV1()
    {
        return Results.Ok(new { Status = "Running." });
    }

    private static IResult CheckHealthV2()
    {
        return Results.Ok(new { Status = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley" });
    }

    private static IResult GetExceptionsFile()
    {
        LinkedList<string> lines = ExceptionFile.ReadFile(_filePath);

        return Results.Ok(lines);
    }
}
