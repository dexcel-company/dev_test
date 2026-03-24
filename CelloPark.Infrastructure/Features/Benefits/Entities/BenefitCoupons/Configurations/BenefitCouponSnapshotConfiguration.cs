using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Benefits.Entities.BenefitCoupons.Configurations;

internal sealed class BenefitCouponSnapshotConfiguration :
    IEntityTypeConfiguration<BenefitCouponSnapshot>
{
    public void Configure(EntityTypeBuilder<BenefitCouponSnapshot> builder)
    {
        builder
            .ToTable("BenefitCouponSnapshot")
            .HasKey(benefitCoupon => benefitCoupon.Id);

        builder
            .Property(benefitCoupon => benefitCoupon.Id)
            .ValueGeneratedNever();

        builder
            .Property(benefitCoupon => benefitCoupon.Coupon)
            .HasMaxLength(20);

        builder
            .Property(benefitCoupon => benefitCoupon.CouponType)
            .HasConversion(DatabaseContextConverters.CouponTypeConverter);
    }
}
