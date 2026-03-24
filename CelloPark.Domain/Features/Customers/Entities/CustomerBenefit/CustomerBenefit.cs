using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Features.Benefits;
using ErrorOr;

namespace CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;

public sealed class CustomerBenefit
{
    private CustomerBenefit() { }

    private CustomerBenefit(
        string customerId,
        Guid benefitId,
        decimal? debit,
        int? frequencyCountLeft,
        decimal? limitAmountLeft,
        DateTime? startDate,
        DateTime? endDate)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BenefitId = benefitId;
        Debit = debit;
        FrequencyCountLeft = frequencyCountLeft;
        LimitAmountLeft = limitAmountLeft;
        Status = Status.Active;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid Id { get; }
    public string CustomerId { get; private set; } = null!;
    public Guid BenefitId { get; private set; }
    public Benefit Benefit { get; private set; } = null!;
    public decimal? Debit { get; private set; }
    public int? FrequencyCountLeft { get; private set; }
    public decimal? LimitAmountLeft { get; private set; }
    public Status Status { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public static ErrorOr<CustomerBenefit> Create(
        string customerId,
        Guid benefitId,
        decimal? debit,
        int? frequencyCountLeft,
        decimal? limitAmountLeft,
        DateTime? startDate,
        DateTime? endDate)
    {
        ErrorOr<decimal?> debitResult = ValidateDebit(debit);
        ErrorOr<int?> frequencyCountLeftResult = ValidateFrequencyCountLeft(frequencyCountLeft);
        ErrorOr<decimal?> limitAmountLeftResult = ValidateLimitAmountLeft(limitAmountLeft);
        ErrorOr<DateTime?> startDateResult = ValidateStartDate(startDate);
        ErrorOr<DateTime?> endDateResult = ValidateEndDate(endDate);

        List<Error> errors = ErrorProvider.Join(
            debitResult,
            frequencyCountLeftResult,
            limitAmountLeftResult,
            startDateResult,
            endDateResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new CustomerBenefit(
            customerId: customerId,
            benefitId: benefitId,
            debit: debitResult.Value,
            frequencyCountLeft: frequencyCountLeftResult.Value,
            limitAmountLeft: limitAmountLeftResult.Value,
            startDate: startDateResult.Value,
            endDate: endDateResult.Value);
    }

    private static ErrorOr<decimal?> ValidateDebit(decimal? debit)
    {
        if (debit is null)
        {
            return debit;
        }

        if (debit < 0.0m)
        {
            return Error.Validation("CustomerBenefit.Debit", $"Debit value '{debit}' is invalid.");
        }

        return debit;
    }

    private static ErrorOr<int?> ValidateFrequencyCountLeft(int? frequencyCountLeft)
    {
        if (frequencyCountLeft is null)
        {
            return frequencyCountLeft;
        }

        if (frequencyCountLeft < 0)
        {
            return Error.Validation("CustomerBenefit.Debit", $"Freequency count left value '{frequencyCountLeft}' is invalid.");
        }

        return frequencyCountLeft;
    }

    private static ErrorOr<decimal?> ValidateLimitAmountLeft(decimal? limitAmountLeft)
    {
        if (limitAmountLeft is null)
        {
            return limitAmountLeft;
        }

        if (limitAmountLeft < 0.0m)
        {
            return Error.Validation("CustomerBenefit.Debit", $"Limit amount left value '{limitAmountLeft}' is invalid.");
        }

        return limitAmountLeft;
    }

    private static ErrorOr<DateTime?> ValidateStartDate(DateTime? startDate)
    {
        if (startDate is null)
        {
            return startDate;
        }

        if (startDate.Value == default || startDate.Value == DateTime.MinValue || startDate.Value == DateTime.MaxValue)
        {
            return Error.Validation("CustomerBenefit.StartDate", $"Start date value '{startDate}' is invalid.");
        }

        return startDate;
    }

    private static ErrorOr<DateTime?> ValidateEndDate(DateTime? endDate)
    {
        if (endDate is null)
        {
            return endDate;
        }

        if (endDate.Value == default || endDate.Value == DateTime.MinValue || endDate.Value == DateTime.MaxValue)
        {
            return Error.Validation("CustomerBenefit.EndDate", $"End date value {endDate} is invalid.");
        }

        return endDate;
    }
}
