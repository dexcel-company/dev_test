using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits.Constants;
using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Domain.Features.Benefits.Enums;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits;

public sealed class BenefitSnapshot
{
    private BenefitSnapshot(
        Guid id,
        string name,
        int? duration,
        int? couponsDuration,
        DateTime? startActiveDate,
        DateTime? endActiveDate,
        DateTime? startPromotionDate,
        DateTime? endPromotionDate)
    {
        Id = id;
        Name = name;
        Duration = duration;
        CouponsDuration = couponsDuration;
        StartActiveDate = startActiveDate;
        EndActiveDate = endActiveDate;
        StartPromotionDate = startPromotionDate;
        EndPromotionDate = endPromotionDate;
    }

    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public int? Duration { get; private set; }
    public int? CouponsDuration { get; private set; }
    public DateTime? StartActiveDate { get; private set; }
    public DateTime? EndActiveDate { get; private set; }
    public DateTime? StartPromotionDate { get; private set; }
    public DateTime? EndPromotionDate { get; private set; }
    public DateOnly SnapshotDate { get; set; }
    public IReadOnlyList<BenefitPaymentCategorySnapshot> PaymentCategories => _paymentCategories.AsReadOnly();
    public IReadOnlyList<BenefitCouponSnapshot> Coupons => _coupons.AsReadOnly();

    private readonly List<BenefitPaymentCategorySnapshot> _paymentCategories = [];
    private readonly List<BenefitCouponSnapshot> _coupons = [];

    public static ErrorOr<BenefitSnapshot> Create(
        Guid id,
        string name,
        DateTime? startActiveDate,
        DateTime? endActiveDate,
        DateTime? startPromotionDate,
        DateTime? endPromotionDate,
        int? duration,
        int? couponsDuration)
    {
        ErrorOr<string> nameResult = ValidateName(name);
        ErrorOr<DateTime?> startActiveDateResult = ValidateStartActiveDate(startActiveDate);
        ErrorOr<DateTime?> endActiveDateResult = ValidateEndActiveDate(endActiveDate);
        ErrorOr<DateTime?> startPromotionDateResult = ValidateStartPromotionDate(startPromotionDate);
        ErrorOr<DateTime?> endPromotionDateResult = ValidateEndPromotionDate(endPromotionDate);
        ErrorOr<int?> durationResult = ValidateDuration(duration);
        ErrorOr<int?> couponsDurationResult = ValidateCouponsDuration(couponsDuration);

        List<Error> errors = ErrorProvider.Join(
            nameResult,
            startActiveDateResult,
            endActiveDateResult,
            startPromotionDateResult,
            endPromotionDateResult,
            durationResult,
            couponsDurationResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new BenefitSnapshot(
            id: id,
            name: nameResult.Value,
            startActiveDate: startActiveDateResult.Value,
            endActiveDate: endActiveDateResult.Value,
            startPromotionDate: startPromotionDateResult.Value,
            endPromotionDate: endPromotionDateResult.Value,
            duration: durationResult.Value,
            couponsDuration: couponsDurationResult.Value);
    }

    public ErrorOr<None> UpdateName(string name)
    {
        ErrorOr<string> nameResult = ValidateName(name);

        if (nameResult.IsError)
        {
            return nameResult.FirstError;
        }

        Name = nameResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateStartActiveDate(DateTime? startActiveDate)
    {
        ErrorOr<DateTime?> startActiveDateResult = ValidateStartActiveDate(startActiveDate);

        if (startActiveDateResult.IsError)
        {
            return startActiveDateResult.FirstError;
        }

        StartActiveDate = startActiveDateResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateEndActiveDate(DateTime? endActiveDate)
    {
        ErrorOr<DateTime?> endActiveDateResult = ValidateEndActiveDate(endActiveDate);

        if (endActiveDateResult.IsError)
        {
            return endActiveDateResult.FirstError;
        }

        EndActiveDate = endActiveDateResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateStartPromotionDate(DateTime? startPromotionDate)
    {
        ErrorOr<DateTime?> startPromotionDateResult = ValidateStartPromotionDate(startPromotionDate);

        if (startPromotionDateResult.IsError)
        {
            return startPromotionDateResult.FirstError;
        }

        StartPromotionDate = startPromotionDateResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateEndPromotionDate(DateTime? endPromotionDate)
    {
        ErrorOr<DateTime?> endPromotionDateResult = ValidateEndPromotionDate(endPromotionDate);

        if (endPromotionDateResult.IsError)
        {
            return endPromotionDateResult.FirstError;
        }

        EndPromotionDate = endPromotionDateResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateDuration(int? duration)
    {
        ErrorOr<int?> durationResult = ValidateDuration(duration);

        if (durationResult.IsError)
        {
            return durationResult.FirstError;
        }

        Duration = durationResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateCouponsDuration(int? couponsDuration)
    {
        ErrorOr<int?> couponsDurationResult = ValidateCouponsDuration(couponsDuration);

        if (couponsDurationResult.IsError)
        {
            return couponsDurationResult.FirstError;
        }

        CouponsDuration = couponsDurationResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddPaymentCategory(
        Guid id,
        Guid? planId,
        Guid? packageId,
        Guid? itemId,
        decimal amount,
        AmountType? amountType,
        int? frequency,
        FrequencyType? frequencyType,
        decimal? amountLimit)
    {
        if (planId is null && packageId is null && itemId is null)
        {
            return Error.Validation("Benefit.PaymentCategory", "Invalid benefit setup.");
        }

        amountType ??= AmountType.None;
        frequencyType ??= FrequencyType.None;

        ErrorOr<BenefitPaymentCategorySnapshot> paymentCategoryResult = BenefitPaymentCategorySnapshot.Create(
            id: id,
            planId: planId,
            packageId: packageId,
            itemId: itemId,
            benefitId: Id,
            amount: amount,
            amountType: amountType,
            frequency: frequency,
            frequencyType: frequencyType,
            AmountLimit: amountLimit);

        if (paymentCategoryResult.IsError)
        {
            return paymentCategoryResult.Errors;
        }

        _paymentCategories.Add(paymentCategoryResult.Value);

        return None.Value;
    }

    //public ErrorOr<None> AddCoupon(Guid id, string coupon, CouponType? couponType)
    //{
    //    couponType ??= CouponType.None;

    //    ErrorOr<BenefitCouponSnapshot> couponResult = BenefitCouponSnapshot.Create(
    //        id: id,
    //        benefitId: Id,
    //        coupon: coupon,
    //        couponType: couponType,
    //        duration: CouponsDuration ?? 0,
    //        status);

    //    if (couponResult.IsError)
    //    {
    //        return couponResult.Errors;
    //    }

    //    _coupons.Add(couponResult.Value);

    //    return None.Value;
    //}

    private static ErrorOr<string> ValidateName(string name)
    {
        if (name is null)
        {
            return BenefitErrors.NameIsNull;
        }

        if (name.Length < BenefitSettings.NameMinLength)
        {
            return BenefitErrors.NameIsTooShort;
        }

        if (name.Length > BenefitSettings.NameMaxLength)
        {
            return BenefitErrors.NameIsTooLong;
        }

        return name;
    }

    private static ErrorOr<DateTime?> ValidateStartActiveDate(DateTime? startActiveDate)
    {
        if (startActiveDate is null)
        {
            return startActiveDate;
        }

        if (startActiveDate.Value.Kind != DateTimeKind.Utc)
        {
            return BenefitErrors.StartActiveDateIsNotUtc;
        }

        if (startActiveDate.Value == default
            || startActiveDate.Value == DateTime.MinValue
            || startActiveDate.Value == DateTime.MaxValue)
        {
            return BenefitErrors.StartActiveDateIsInvalid;
        }

        return startActiveDate;
    }

    private static ErrorOr<DateTime?> ValidateEndActiveDate(DateTime? endActiveDate)
    {
        if (endActiveDate is null)
        {
            return endActiveDate;
        }

        if (endActiveDate.Value.Kind != DateTimeKind.Utc)
        {
            return BenefitErrors.EndActiveDateIsNotUtc;
        }

        if (endActiveDate.Value == default
            || endActiveDate.Value == DateTime.MinValue
            || endActiveDate.Value == DateTime.MaxValue)
        {
            return BenefitErrors.EndActiveDateIsInvalid;
        }

        return endActiveDate;
    }

    private static ErrorOr<DateTime?> ValidateStartPromotionDate(DateTime? startPromotionDate)
    {
        if (startPromotionDate is null)
        {
            return startPromotionDate;
        }

        if (startPromotionDate.Value.Kind != DateTimeKind.Utc)
        {
            return BenefitErrors.StartPromotionDateIsNotUtc;
        }

        if (startPromotionDate.Value == default
            || startPromotionDate.Value == DateTime.MinValue
            || startPromotionDate.Value == DateTime.MaxValue)
        {
            return BenefitErrors.StartPromotionDateIsInvalid;
        }

        return startPromotionDate;
    }

    private static ErrorOr<DateTime?> ValidateEndPromotionDate(DateTime? endPromotionDate)
    {
        if (endPromotionDate is null)
        {
            return endPromotionDate;
        }

        if (endPromotionDate.Value.Kind != DateTimeKind.Utc)
        {
            return BenefitErrors.EndPromotionDateIsNotUtc;
        }

        if (endPromotionDate.Value == default
            || endPromotionDate.Value == DateTime.MinValue
            || endPromotionDate.Value == DateTime.MaxValue)
        {
            return BenefitErrors.EndPromotionDateIsInvalid;
        }

        return endPromotionDate;
    }

    private static ErrorOr<int?> ValidateDuration(int? duration)
    {
        if (duration is null)
        {
            return duration;
        }

        // TODO

        return duration;
    }

    private static ErrorOr<int?> ValidateCouponsDuration(int? couponsDuration)
    {
        if (couponsDuration is null)
        {
            return couponsDuration;
        }

        // TODO

        return couponsDuration;
    }
}
