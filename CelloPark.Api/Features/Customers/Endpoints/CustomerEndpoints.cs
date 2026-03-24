using Asp.Versioning;
using Asp.Versioning.Builder;
using CelloPark.Api.Common.Endpoints;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create.Abstractions;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.Update.Abstractions;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Commands.UpdatePackage.Abstractions;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetAllPackages.Abstractions;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Queries.GetById.Abstractions;
using CelloPark.Application.Features.Customers.Queries.GetAll;
using CelloPark.Application.Features.Customers.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Customers.Queries.GetById;
using CelloPark.Application.Features.Customers.Queries.GetById.Abstractions;
using CelloPark.Domain.Common.Results;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CelloPark.Api.Features.Customers.Endpoints;

public static class CustomerEndpoints
{
    public static void AddCustomerEndpoints(this IEndpointRouteBuilder builder)
    {
        IVersionedEndpointRouteBuilder versionedApi = builder
            .NewVersionedApi("Customers");

        RouteGroupBuilder groupV1 = versionedApi
            .MapGroup("/api/v{version:ApiVersion}/customers")
            .RequireAuthorization()
            .HasApiVersion(new ApiVersion(1, 0));

        groupV1
            .MapGet("/", GetAllCustomers)
            .Produces<Page<CustomerViewPageDto>>(StatusCodes.Status200OK)
            .WithName(nameof(GetAllCustomers))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all customers.",
            });

        groupV1
            .MapGet("/{customerId:guid}", GetCustomerById)
            .Produces<CustomerGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetCustomerById))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a customer by id.",
            });

        groupV1
            .MapGet("/{customerId:guid}/plans", GetCustomerPlan)
            .Produces<CustomerPlanGetDto>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetCustomerPlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a customer plan",
            });

        groupV1
            .MapPut("/{customerId:guid}/plans/{customerPlanId:guid}", UpdateCustomerPlan)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(UpdateCustomerPlan))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a customer plan",
            });

        groupV1
            .MapGet("/{customerId:guid}/plans/{customerPlanId:guid}/packages", GetCustomerPackages)
            .Produces<Page<CustomerPackagePageDto>>(StatusCodes.Status200OK)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(GetCustomerPackages))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get all customer plan packages",
            });

        groupV1
            .MapPut("/{customerId:guid}/plans/{customerPlanId:guid}/packages/{CustomerPackageId:guid}", UdpateCustomerPackage)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(UdpateCustomerPackage))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Update a customer plan package",
            });

        groupV1
            .MapPost("/{customerId}/coupon-usages", CreateCustomerCouponUsage)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ValidationProblem>(StatusCodes.Status400BadRequest)
            .Produces<ProblemHttpResult>(StatusCodes.Status404NotFound)
            .WithName(nameof(CreateCustomerCouponUsage))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a customer coupon usage",
            });
    }

    private static async Task<IResult> GetAllCustomers(
        [AsParameters] PaginationCriteria paginationCriteria,
        [AsParameters] SortingCriteria sortingCriteria,
        [AsParameters] CustomerFilteringQuery filteringCriteria,
        [FromServices] IGetAllCustomersQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllCustomersQuery request = new(paginationCriteria, sortingCriteria, filteringCriteria);
        Page<CustomerPageDto> result = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCustomerById(
        [FromRoute] Guid customerId,
        [FromServices] IGetCustomerByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetCustomerByIdQuery request = new(customerId);
        ErrorOr<CustomerGetDto> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> GetCustomerPlan(
        [FromRoute] Guid customerId,
        [FromServices] IGetCustomerPlanByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetCustomerPlanByIdQuery request = new(customerId);
        ErrorOr<CustomerPlanGetDto> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> UpdateCustomerPlan(
        [FromRoute] Guid customerId,
        [FromRoute] Guid customerPlanId,
        [FromBody] CustomerPlanUpdateDto dto,
        [FromServices] IUpdateCustomerPlanCommandHandler handler,
        CancellationToken cancellationToken)
    {
        UpdateCustomerPlanCommand request = new(customerId, customerPlanId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> GetCustomerPackages(
        [FromRoute] Guid customerId,
        [FromRoute] Guid customerPlanId,
        [AsParameters] PaginationCriteria paginationCriteria,
        [FromServices] IGetAllCustomerPackagesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        GetAllCustomerPackagesQuery request = new(customerId, customerPlanId, paginationCriteria);
        ErrorOr<Page<CustomerPackagePageDto>> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            value => Results.Ok(value),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> UdpateCustomerPackage(
        [FromRoute] Guid customerId,
        [FromRoute] Guid customerPlanId,
        [FromRoute] Guid CustomerPackageId,
        [FromBody] CustomerPackageUpdateDto dto,
        [FromServices] IUpdateCustomerPackageCommandHandler handler,
        CancellationToken cancellationToken)
    {
        UpdateCustomerPackageCommand request = new(customerId, customerPlanId, CustomerPackageId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }

    private static async Task<IResult> CreateCustomerCouponUsage(
        [FromRoute] string customerId,
        [FromBody] CustomerCouponUsageCreateDto dto,
        [FromServices] IValidator<CustomerCouponUsageCreateDto> validator,
        [FromServices] ICreateCustomerCoupoUsageQuerydHandler handler,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(dto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        CreateCustomerCoupoUsageQuery request = new(customerId, dto);
        ErrorOr<None> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            _ => Results.NoContent(),
            error => ErrorResults.Problem(error));
    }
}
