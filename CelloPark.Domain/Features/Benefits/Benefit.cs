using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;
using CelloPark.Domain.Features.Benefits.Constants;
using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Domain.Features.Benefits.Enums;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits;

public sealed class Benefit :
    ICreateDetailsOwner, IUpdateDetailsOwner, IDeleteDetailsOwner, IStatusOwner
{
    private Benefit() { }

    private Benefit(
        string name,
        string? description,
        DateTime? startActiveDate,
        DateTime? endActiveDate,
        DateTime? startPromotionDate,
        DateTime? endPromotionDate,
        int? duration,
        int? couponsDuration)
    {
        Name = name;
        Description = description;
        StartActiveDate = startActiveDate;
        EndActiveDate = endActiveDate;
        StartPromotionDate = startPromotionDate;
        EndPromotionDate = endPromotionDate;
        Duration = duration;
        CouponsDuration = couponsDuration;
        Status = Status.Inactive;
    }

    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public DateTime? StartActiveDate { get; private set; }
    public DateTime? EndActiveDate { get; private set; }
    public DateTime? StartPromotionDate { get; private set; }
    public DateTime? EndPromotionDate { get; private set; }
    public int? Duration { get; private set; }
    public int? CouponsDuration { get; private set; }
    public Status Status { get; private set; }
    public CreateDetails CreateDetails { get; private set; } = null!;
    public UpdateDetails UpdateDetails { get; private set; } = null!;
    public DeleteDetails DeleteDetails { get; private set; } = null!;
    public IReadOnlyList<BenefitPaymentCategory> PaymentCategories => _paymentCategories.AsReadOnly();
    public IReadOnlyList<BenefitCoupon> Coupons => _coupons.AsReadOnly();

    private readonly List<BenefitPaymentCategory> _paymentCategories = [];
    private readonly List<BenefitCoupon> _coupons = [];

    public static ErrorOr<Benefit> Create(
        string name,
        string? description,
        DateTime? startActiveDate,
        DateTime? endActiveDate,
        DateTime? startPromotionDate,
        DateTime? endPromotionDate,
        int? duration,
        int? couponsDuration)
    {
        ErrorOr<string> nameResult = ValidateName(name);
        ErrorOr<string?> descriptionResult = ValidateDescription(description);
        ErrorOr<DateTime?> startActiveDateResult = ValidateStartActiveDate(startActiveDate);
        ErrorOr<DateTime?> endActiveDateResult = ValidateEndActiveDate(endActiveDate);
        ErrorOr<DateTime?> startPromotionDateResult = ValidateStartPromotionDate(startPromotionDate);
        ErrorOr<DateTime?> endPromotionDateResult = ValidateEndPromotionDate(endPromotionDate);
        ErrorOr<int?> durationResult = ValidateDuration(duration);
        ErrorOr<int?> couponsDurationResult = ValidateCouponsDuration(couponsDuration);

        List<Error> errors = ErrorProvider.Join(
            nameResult,
            descriptionResult,
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

        return new Benefit(
            name: nameResult.Value,
            description: descriptionResult.Value,
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

    public ErrorOr<None> UpdateDescription(string? description)
    {
        ErrorOr<string?> descriptionResult = ValidateDescription(description);

        if (descriptionResult.IsError)
        {
            return descriptionResult.FirstError;
        }

        Description = descriptionResult.Value;

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

        ErrorOr<BenefitPaymentCategory> paymentCategoryResult = BenefitPaymentCategory.Create(
            planId: planId,
            packageId: packageId,
            itemId: itemId,
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

    public void ClearPaymentCategories()
    {
        foreach (BenefitPaymentCategory paymentCategory in _paymentCategories)
        {
            paymentCategory.MarkAsDeleted();
        }
    }

    public void ClearCoupons()
    {
        foreach (BenefitCoupon coupon in _coupons)
        {
            coupon.MarkAsDeleted();
        }
    }

    public ErrorOr<None> AddCoupon(string coupon, CouponType? couponType)
    {
        couponType ??= CouponType.None;

        ErrorOr<BenefitCoupon> couponResult = BenefitCoupon.Create(
            benefitId: Id,
            coupon: coupon,
            couponType: couponType,
            duration: CouponsDuration ?? 0);

        if (couponResult.IsError)
        {
            return couponResult.Errors;
        }

        if (_coupons.Any(benefitCoupon => benefitCoupon.Coupon.Equals(coupon, StringComparison.InvariantCultureIgnoreCase)))
        {
            return Error.Validation("BenefitCoupon.Coupon", $"Benefit already contains coupon '{coupon}'.");
        }

        _coupons.Add(couponResult.Value);

        return None.Value;
    }

    public ErrorOr<None> AddCreateDetails(DateTime? createdAt, Guid? createdBy)
    {
        ErrorOr<CreateDetails> creationDetailResult = CreateDetails.Create(createdAt, createdBy);

        if (creationDetailResult.IsError)
        {
            return creationDetailResult.Errors;
        }

        CreateDetails = creationDetailResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddUpdateDetails(DateTime? updatedAt, Guid? updatedBy)
    {
        ErrorOr<UpdateDetails> updateDetailsResult = UpdateDetails.Create(updatedAt, updatedBy);

        if (updateDetailsResult.IsError)
        {
            return updateDetailsResult.Errors;
        }

        UpdateDetails = updateDetailsResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddDeleteDetails(DateTime? deletedAt, Guid? deletedBy)
    {
        ErrorOr<DeleteDetails> deleteDetailsResult = DeleteDetails.Create(deletedAt, deletedBy);

        if (deleteDetailsResult.IsError)
        {
            return deleteDetailsResult.Errors;
        }

        DeleteDetails = deleteDetailsResult.Value;

        return None.Value;
    }

    public void MarkAsActive()
    {
        Status = Status.Active;

        foreach (BenefitCoupon coupon in _coupons)
        {
            coupon.MarkAsActive();
        }

        foreach (BenefitPaymentCategory paymentCategory in _paymentCategories)
        {
            paymentCategory.MarkAsActive();
        }
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;

        foreach (BenefitCoupon coupon in _coupons)
        {
            coupon.MarkAsInactive();
        }

        foreach (BenefitPaymentCategory paymentCategory in _paymentCategories)
        {
            paymentCategory.MarkAsInactive();
        }
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;

        foreach (BenefitCoupon coupon in _coupons)
        {
            coupon.MarkAsDeleted();
        }

        foreach (BenefitPaymentCategory paymentCategory in _paymentCategories)
        {
            paymentCategory.MarkAsDeleted();
        }
    }

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

    private static ErrorOr<string?> ValidateDescription(string? description)
    {
        if (description is null)
        {
            return description;
        }

        if (description.Length > BenefitSettings.DescriptionMaxLength)
        {
            return BenefitErrors.DescriptionIsTooLong;
        }

        return description;
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
