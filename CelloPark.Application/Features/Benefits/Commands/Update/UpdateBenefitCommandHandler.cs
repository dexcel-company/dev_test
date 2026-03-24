using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Commands.Update.Abstractions;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Benefits.Extensions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Enums;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Commands.Update;

internal sealed class UpdateBenefitCommandHandler :
    IUpdateBenefitCommandHandler
{
    private readonly IManagementContext _managementContext;

    public UpdateBenefitCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    public async Task<ErrorOr<None>> HandleAsync(
        UpdateBenefitCommand request, CancellationToken cancellationToken = default)
    {
        bool exists = await _managementContext.Benefits
            .AnyAsync(benefit => benefit.Name == request.Dto.Name && benefit.Id != request.BenefitId, cancellationToken);

        if (exists)
        {
            return BenefitErrors.NameAlreadyExists;
        }

        Benefit? benefit = await _managementContext.Benefits
            .Include(benefit => benefit.Coupons)
            .Include(benefit => benefit.PaymentCategories)
            .FirstOrDefaultAsync(benefit => benefit.Id == request.BenefitId, cancellationToken);

        if (benefit is null)
        {
            return BenefitErrors.NotFound;
        }

        ErrorOr<Benefit> benefitResult = benefit.Update(request.Dto);

        if (benefitResult.IsError)
        {
            return benefitResult.Errors;
        }

        benefitResult.Value.ClearCoupons();

        foreach (BenefitCouponUpdateDto coupon in request.Dto.Coupons)
        {
            CouponType? couponType = CouponType.FromKey(coupon.CouponType);

            ErrorOr<None> couponResult = benefitResult.Value.AddCoupon(coupon.Coupon, couponType);

            if (couponResult.IsError)
            {
                return couponResult.Errors;
            }
        }

        benefitResult.Value.ClearPaymentCategories();

        foreach (BenefitPaymentCategoryUpdateDto paymentCategory in request.Dto.PaymentCategories)
        {
            AmountType? amountType = AmountType.FromKey(paymentCategory.AmountType);
            FrequencyType? frequencyType = FrequencyType.FromKey(paymentCategory.FrequencyType);

            ErrorOr<None> paymentCategoryResult = benefitResult.Value.AddPaymentCategory(
                planId: paymentCategory.Plan,
                packageId: paymentCategory.Package,
                itemId: paymentCategory.Item,
                amount: paymentCategory.Amount,
                amountType: amountType,
                frequency: paymentCategory.Frequency,
                frequencyType: frequencyType,
                amountLimit: paymentCategory.AmountLimit);

            if (paymentCategoryResult.IsError)
            {
                return paymentCategoryResult.Errors;
            }
        }

        await _managementContext.SaveChangesAsync(cancellationToken);

        return None.Value;
    }
}
