using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Benefits;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;

public sealed class CustomerCouponUsage
{
    private CustomerCouponUsage() { }

    private CustomerCouponUsage(
        string coupon,
        string customerId,
        Guid benefitId,
        DateOnly startDate,
        DateOnly endDate)
    {
        Coupon = coupon;
        CustomerId = customerId;
        BenefitId = benefitId;
        Status = Status.Active;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; }
    public string Coupon { get; private set; } = null!;
    public string CustomerId { get; private set; } = null!;
    public Guid BenefitId { get; private set; }
    public Benefit Benefit { get; private set; } = null!;
    public Status Status { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    public static ErrorOr<CustomerCouponUsage> Create(
        string coupon,
        string customerId,
        Guid benefitid,
        DateOnly startDate,
        DateOnly endDate)
    {
        ErrorOr<string> couponResult = ValidateCoupon(coupon);
        ErrorOr<string> customerIdResult = ValidateCustomerId(customerId);
        ErrorOr<Guid> benefitIdResult = ValidateBenefitId(benefitid);
        ErrorOr<DateOnly> startDateResult = ValidateStartDate(startDate);
        ErrorOr<DateOnly> endDateResult = ValidateEndDate(endDate);

        List<Error> errors = ErrorProvider.Join(
            couponResult,
            customerIdResult,
            startDateResult,
            endDateResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new CustomerCouponUsage(
            coupon: couponResult.Value,
            customerId: customerIdResult.Value,
            benefitId: benefitIdResult.Value,
            startDate: startDateResult.Value,
            endDate: endDateResult.Value);
    }

    private static ErrorOr<string> ValidateCoupon(string coupon)
    {
        // TODO

        return coupon;
    }

    private static ErrorOr<string> ValidateCustomerId(string customerId)
    {
        // TODO

        return customerId;
    }

    private static ErrorOr<Guid> ValidateBenefitId(Guid benefitId)
    {
        // TODO

        return benefitId;
    }

    private static ErrorOr<DateOnly> ValidateStartDate(DateOnly startDate)
    {
        // TODO

        return startDate;
    }

    private static ErrorOr<DateOnly> ValidateEndDate(DateOnly endDate)
    {
        // TODO

        return endDate;
    }
}
