using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Responses;
using CelloPark.Application.Features.Benefits.Commands.Create.Abstractions;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Benefits.Extensions;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Enums;
using CelloPark.Domain.Features.Benefits.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Commands.Create;

internal sealed class CreateBenefitCommandHandler :
    ICreateBenefitCommandHandler
{
    public CreateBenefitCommandHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<IdResult>> HandleAsync(
        CreateBenefitCommand request, CancellationToken cancellationToken = default)
    {
        ErrorOr<Benefit> benefitResult = request.Dto.ToModel();

        if (benefitResult.IsError)
        {
            return benefitResult.Errors;
        }

        bool exists = await _managementContext.Benefits
            .AnyAsync(benefit => benefit.Name == request.Dto.Name, cancellationToken);

        if (exists)
        {
            return BenefitErrors.NameAlreadyExists;
        }

        foreach (BenefitCouponCreateDto coupon in request.Dto.Coupons)
        {
            CouponType? couponType = CouponType.FromKey(coupon.CouponType);

            ErrorOr<None> couponResult = benefitResult.Value.AddCoupon(coupon.Coupon, couponType);

            if (couponResult.IsError)
            {
                return couponResult.Errors;
            }
        }

        foreach (BenefitPaymentCategoryCreateDto paymentCategory in request.Dto.PaymentCategories)
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

        await _managementContext.Benefits.AddAsync(benefitResult.Value, cancellationToken);
        await _managementContext.SaveChangesAsync(cancellationToken);

        return new IdResult(benefitResult.Value.Id);
    }
}
