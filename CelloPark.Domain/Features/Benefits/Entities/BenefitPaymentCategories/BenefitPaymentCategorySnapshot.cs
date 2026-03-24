using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits.Enums;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;

public sealed class BenefitPaymentCategorySnapshot
{
    private BenefitPaymentCategorySnapshot(
        Guid id,
        Guid? planId,
        Guid? packageId,
        Guid? itemId,
        Guid benefitId,
        decimal amount,
        AmountType amountType,
        int? frequency,
        FrequencyType frequencyType,
        decimal? amountLimit)
    {
        Id = id;
        PlanId = planId;
        PackageId = packageId;
        ItemId = itemId;
        BenefitId = benefitId;
        Amount = amount;
        AmountType = amountType;
        Frequency = frequency;
        FrequencyType = frequencyType;
        AmountLimit = amountLimit;
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
    public DateOnly SnapshotDate { get; set; }

    public static ErrorOr<BenefitPaymentCategorySnapshot> Create(
        Guid id,
        Guid? planId,
        Guid? packageId,
        Guid? itemId,
        Guid benefitId,
        decimal amount,
        AmountType amountType,
        int? frequency,
        FrequencyType frequencyType,
        decimal? AmountLimit)
    {
        return new BenefitPaymentCategorySnapshot(
            id: id,
            planId: planId,
            packageId: packageId,
            itemId: itemId,
            benefitId: benefitId,
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
}
