using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;
using FluentValidation;

namespace CelloPark.Api.Features.Customers.Validators;

public sealed class CreateCouponUsageValidator :
    AbstractValidator<CustomerCouponUsageCreateDto>
{
    public CreateCouponUsageValidator()
    {
        RuleFor(x => x.Coupon)
            .MinimumLength(5)
            .WithMessage("Coupon cannot be less than 5 characters.")
            .MaximumLength(20)
            .WithMessage("Coupon cannot be greater than 20 characters.");
    }
}
