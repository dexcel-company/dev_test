using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;
using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerCouponUsages.Configurations;

internal sealed class CustomerCouponUsageSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerCouponUsageSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerCouponUsageSnapshot> builder)
    {
        builder
            .ToTable("CustomerCouponUsageSnapshot")
            .HasKey(customerCouponUsage => customerCouponUsage.Id);

        builder
            .Property(customerCouponUsage => customerCouponUsage.Id)
            .ValueGeneratedNever();

        builder
            .Property(customerCouponUsage => customerCouponUsage.Coupon)
            .HasMaxLength(CustomerCouponUsageSettings.CouponMaxLength);

        ConfigureBenefit(builder);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<CustomerCouponUsageSnapshot> builder)
    {
        builder
            .HasOne(customerCouponUsage => customerCouponUsage.Benefit)
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(customerCouponUsage => customerCouponUsage.BenefitId);
    }
}
