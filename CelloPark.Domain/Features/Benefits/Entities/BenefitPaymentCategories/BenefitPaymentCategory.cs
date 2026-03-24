using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;
using CelloPark.Domain.Features.Benefits.Enums;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;

public sealed class BenefitPaymentCategory :
    IStatusOwner, ICreateDetailsOwner, IUpdateDetailsOwner, IDeleteDetailsOwner
{
    private BenefitPaymentCategory() { }

    private BenefitPaymentCategory(
        Guid? planId,
        Guid? packageId,
        Guid? itemId,
        decimal amount,
        AmountType amountType,
        int? frequency,
        FrequencyType frequencyType,
        decimal? amountLimit)
    {
        PlanId = planId;
        PackageId = packageId;
        ItemId = itemId;
        Amount = amount;
        AmountType = amountType;
        Frequency = frequency;
        FrequencyType = frequencyType;
        AmountLimit = amountLimit;
        Status = Status.Active;
    }

    public Guid Id { get; }
    public Guid? PlanId { get; private set; }
    public Guid? PackageId { get; private set; }
    public Guid? ItemId { get; private set; }
    public Guid BenefitId { get; private set; }
    public decimal Amount { get; private set; }
    public AmountType AmountType { get; private set; } = null!;
    public int? Frequency { get; private set; }
    public FrequencyType FrequencyType { get; private set; } = null!;
    public decimal? AmountLimit { get; private set; }
    public Status Status { get; private set; }
    public CreateDetails CreateDetails { get; private set; } = null!;
    public UpdateDetails UpdateDetails { get; private set; } = null!;
    public DeleteDetails DeleteDetails { get; private set; } = null!;

    public static ErrorOr<BenefitPaymentCategory> Create(
        Guid? planId,
        Guid? packageId,
        Guid? itemId,
        decimal amount,
        AmountType amountType,
        int? frequency,
        FrequencyType frequencyType,
        decimal? AmountLimit)
    {
        return new BenefitPaymentCategory(
            planId: planId,
            packageId: packageId,
            itemId: itemId,
            amount: amount,
            amountType: amountType,
            frequency: frequency,
            frequencyType: frequencyType,
            amountLimit: AmountLimit);
    }

    public ErrorOr<None> UpdatePlan(Guid? planId)
    {
        PlanId = planId;

        return None.Value;
    }

    public ErrorOr<None> UpdatePackage(Guid? packageId)
    {
        PackageId = packageId;

        return None.Value;
    }

    public ErrorOr<None> UpdateItem(Guid? itemId)
    {
        ItemId = itemId;

        return None.Value;
    }

    public ErrorOr<None> UpdateAmount(decimal amount)
    {
        Amount = amount;

        return None.Value;
    }

    public ErrorOr<None> UpdateAmountType(AmountType amountType)
    {
        AmountType = amountType;

        return None.Value;
    }

    public ErrorOr<None> UpdateFrequency(int? frequency)
    {
        Frequency = frequency;

        return None.Value;
    }

    public ErrorOr<None> UpdateFrequencyType(FrequencyType frequencyType)
    {
        FrequencyType = frequencyType;

        return None.Value;
    }

    public ErrorOr<None> UpdateLimitAmount(decimal? limitAmount)
    {
        AmountLimit = limitAmount;

        return None.Value;
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;
    }

    public void MarkAsActive()
    {
        Status = Status.Active;
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;
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
