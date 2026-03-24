using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;
using CelloPark.Domain.Features.Benefits.Enums;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;

public sealed class BenefitCoupon :
    IStatusOwner, ICreateDetailsOwner, IUpdateDetailsOwner, IDeleteDetailsOwner
{
    private BenefitCoupon() { }

    private BenefitCoupon(
        Guid benefitId,
        string coupon,
        CouponType couponType,
        int duration)
    {
        BenefitId = benefitId;
        Coupon = coupon;
        CouponType = couponType;
        Duration = duration;
    }

    public Guid Id { get; }
    public Guid BenefitId { get; private set; }
    public Benefit Benefit { get; private set; } = null!;
    public string Coupon { get; private set; } = null!;
    public CouponType CouponType { get; private set; } = null!;
    public int Duration { get; private set; }
    public Status Status { get; private set; }
    public CreateDetails CreateDetails { get; private set; } = null!;
    public UpdateDetails UpdateDetails { get; private set; } = null!;
    public DeleteDetails DeleteDetails { get; private set; } = null!;

    public static ErrorOr<BenefitCoupon> Create(
        Guid benefitId,
        string coupon,
        CouponType couponType,
        int duration)
    {
        return new BenefitCoupon(benefitId, coupon, couponType, duration);
    }

    public void MarkAsActive()
    {
        Status = Status.Active;
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;
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
}
