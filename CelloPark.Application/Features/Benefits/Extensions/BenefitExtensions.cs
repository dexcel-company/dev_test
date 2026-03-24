using CelloPark.Application.Common.Filtering.Extensions;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Extensions;

public static class BenefitExtensions
{
    public static ErrorOr<Benefit> ToModel(this BenefitCreateDto dto)
    {
        ErrorOr<Benefit> benefitResult = Benefit.Create(
            name: dto.Name,
            description: dto.Description,
            startActiveDate: dto.StartActiveDate,
            endActiveDate: dto.EndActiveDate,
            startPromotionDate: dto.StartPromotionDate,
            endPromotionDate: dto.EndPromotionDate,
            duration: dto.ActivationDateDuration,
            couponsDuration: dto.CouponDateDuration);

        if (benefitResult.IsError)
        {
            return benefitResult.Errors;
        }

        return benefitResult.Value;
    }

    public static ErrorOr<Benefit> Update(this Benefit model, BenefitUpdateDto dto)
    {
        ErrorOr<None> nameResult = model.UpdateName(dto.Name);
        ErrorOr<None> descriptionResult = model.UpdateDescription(dto.Description);
        ErrorOr<None> startActiveDateResult = model.UpdateStartActiveDate(dto.StartActiveDate);
        ErrorOr<None> endActiveDateResult = model.UpdateEndActiveDate(dto.EndActiveDate);
        ErrorOr<None> startPromotionDateResult = model.UpdateStartPromotionDate(dto.StartPromotionDate);
        ErrorOr<None> endPromotionDateResult = model.UpdateEndPromotionDate(dto.EndPromotionDate);
        ErrorOr<None> activationDateDurationResult = model.UpdateDuration(dto.ActivationDateDuration);
        ErrorOr<None> couponDateDurationResult = model.UpdateCouponsDuration(dto.CouponDateDuration);

        List<Error> errors = ErrorProvider.Join(
            nameResult,
            descriptionResult,
            startActiveDateResult,
            endActiveDateResult,
            startPromotionDateResult,
            endPromotionDateResult,
            activationDateDurationResult,
            couponDateDurationResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return model;
    }

    public static IQueryable<Benefit> ApplyFiltering(
        this IQueryable<Benefit> source, BenefitFilteringCriteria filteringCriteria)
    {
        if (!string.IsNullOrWhiteSpace(filteringCriteria.Status)
            && Enum.TryParse(filteringCriteria.Status, true, out Status status))
        {
            if (Enum.IsDefined(status))
            {
                source = source
                    .IgnoreQueryFilters()
                    .Where(benefit => benefit.Status == status);
            }
        }

        if (!string.IsNullOrWhiteSpace(filteringCriteria.Search))
        {
            source = source
                .Where(benefit => EF.Functions.Like(benefit.Name, $"%{filteringCriteria.Search}%"));
        }

        return source;
    }

    public static IOrderedQueryable<Benefit> ApplySorting(
        this IQueryable<Benefit> source, SortingCriteria sortingCriteria)
    {
        if (string.IsNullOrWhiteSpace(sortingCriteria.Sort))
        {
            return source.OrderBy(benefit => benefit.Id);
        }

        return sortingCriteria.Sort switch
        {
            _ when string.Equals(nameof(Benefit.Name), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefit => benefit.Name, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Benefit.EndPromotionDate), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefit => benefit.EndPromotionDate, sortingCriteria.SortMethod).ThenBy(benefit => benefit.Id),
            _ when string.Equals(nameof(Benefit.Status), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefit => benefit.Status, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Benefit.CreateDetails.CreatedAt), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefit => benefit.CreateDetails.CreatedAt, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Benefit.CreateDetails.CreatedBy), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(benefit => benefit.CreateDetails.CreatedBy, sortingCriteria.SortMethod),
            _ =>
                source.OrderBy(benefit => benefit.Id),
        };
    }

    public static IQueryable<BenefitCalculationDto> AsCalculationDto(
        this IQueryable<Benefit> source)
    {
        return source
            .Select(benefit => new BenefitCalculationDto
            {
                Id = benefit.Id,
                Name = benefit.Name,
                StartActiveDate = benefit.StartActiveDate,
                EndActiveDate = benefit.EndActiveDate,
                StartPromotionDate = benefit.StartPromotionDate,
                EndPromotionDate = benefit.EndPromotionDate,
                Duration = benefit.Duration,
                CouponsDuration = benefit.CouponsDuration,
                PaymentCategories = benefit.PaymentCategories
                    .Select(paymentCategory => new BenefitPaymentCategoryCalculationDto
                    {
                        Id = paymentCategory.Id,
                        ItemId = paymentCategory.ItemId,
                        PlanId = paymentCategory.PlanId,
                        PackageId = paymentCategory.PackageId,
                        BenefitId = paymentCategory.BenefitId,
                        Amount = paymentCategory.Amount,
                        AmountLimit = paymentCategory.AmountLimit,
                        AmountType = paymentCategory.AmountType,
                        Frequency = paymentCategory.Frequency,
                        FrequencyType = paymentCategory.FrequencyType,
                    })
                    .ToList(),
                Coupons = benefit.Coupons
                    .Select(coupon => new BenefitCouponCalculationDto
                    {
                        Id = coupon.Id,
                        BenefitId = coupon.BenefitId,
                        Coupon = coupon.Coupon,
                        CouponType = coupon.CouponType,
                        Duration = coupon.Duration,
                        Status = coupon.Status,
                    })
                    .ToList(),
            });
    }
}
