using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Benefits.Queries.GetById.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Queries.GetById;

internal sealed class GetBenefitByIdQueryHandler :
    IGetBenefitByIdQueryHandler
{
    public GetBenefitByIdQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<BenefitGetDto>> HandleAsync(
        GetBenefitByIdQuery request, CancellationToken cancellationToken = default)
    {
        BenefitGetDto? benefitGetDto = await _managementContext.Benefits
            .Where(benefit => benefit.Id == request.BenefitId)
            .Select(benefit => new BenefitGetDto
            {
                Id = benefit.Id,
                Name = benefit.Name,
                Description = benefit.Description,
                StartActiveDate = benefit.StartActiveDate,
                EndActiveDate = benefit.EndActiveDate,
                StartPromotionDate = benefit.StartPromotionDate,
                EndPromotionDate = benefit.EndPromotionDate,
                ActivationDateDuration = benefit.Duration,
                CouponDateDuration = benefit.CouponsDuration,
                Coupons = benefit.Coupons.Select(coupon => new BenefitCouponPageDto
                {
                    Id = coupon.Id,
                    Coupon = coupon.Coupon,
                    CouponType = coupon.CouponType,
                    Duration = coupon.Duration,
                    Status = coupon.Status.ToString(),
                    IsUsed = _managementContext.CustomerCouponUsages
                        .Any(couponUsage => couponUsage.Coupon == coupon.Coupon),
                    Benefit = null!,
                }).ToList(),
                PaymentCategories = benefit.PaymentCategories.Select(paymentCategory => new BenefitPaymentCategoryPageDto
                {
                    Plan = paymentCategory.PlanId,
                    Package = paymentCategory.PackageId,
                    Item = paymentCategory.ItemId,
                    Amount = paymentCategory.Amount,
                    AmountType = paymentCategory.AmountType,
                    Frequency = paymentCategory.Frequency,
                    FrequencyType = paymentCategory.FrequencyType,
                    AmountLimit = paymentCategory.AmountLimit
                }).ToList(),
                CreatedAt = benefit.CreateDetails.CreatedAt,
                CreatedBy = benefit.CreateDetails.User == null ? null : new UserAuditDto
                {
                    Id = benefit.CreateDetails.User.Id,
                    FirstName = benefit.CreateDetails.User.FirstName,
                    LastName = benefit.CreateDetails.User.LastName,
                },
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (benefitGetDto is null)
        {
            return BenefitErrors.NotFound;
        }

        return benefitGetDto;
    }
}
