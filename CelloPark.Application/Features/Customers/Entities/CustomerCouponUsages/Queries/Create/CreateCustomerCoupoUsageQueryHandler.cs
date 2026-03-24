using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create.Abstractions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Queries.Create;

internal sealed class CreateCustomerCoupoUsageQueryHandler :
    ICreateCustomerCoupoUsageQuerydHandler
{
    public CreateCustomerCoupoUsageQueryHandler(
        IManagementContext managementContext,
        TimeProvider timeProvider)
    {
        _managementContext = managementContext;
        _timeProvider = timeProvider;
    }

    private readonly IManagementContext _managementContext;
    private readonly TimeProvider _timeProvider;

    public async Task<ErrorOr<None>> HandleAsync(
        CreateCustomerCoupoUsageQuery request, CancellationToken cancellationToken = default)
    {
        Customer? customer = await _managementContext.Customers
            .Include(customer => customer.CouponUsages)
            .FirstOrDefaultAsync(customer => customer.ShadowId == request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return CustomerErrors.NotFound;
        }

        Benefit? benefit = await _managementContext.Benefits
            .FirstOrDefaultAsync(benefit => benefit.Coupons
                .Any(benefitCoupon => EF.Functions.Like(benefitCoupon.Coupon, $"%{request.Dto.Coupon}%")), cancellationToken);

        if (benefit is null)
        {
            return Error.NotFound("Benefit.NotFound", $"Benefit cannot be found by coupon: '{request.Dto.Coupon}'.");
        }

        DateTimeOffset utcNow = _timeProvider.GetUtcNow();
        DateOnly startDate = DateOnly.FromDateTime(utcNow.DateTime);
        DateOnly endDate = startDate.AddMonths(benefit.CouponsDuration ?? 1);

        ErrorOr<None> couponUsageResult = customer.AddCouponUsage(
            benefitId: benefit.Id,
            coupon: request.Dto.Coupon,
            startDate: startDate,
            endDate: endDate);

        if (couponUsageResult.IsError)
        {
            return couponUsageResult.FirstError;
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
