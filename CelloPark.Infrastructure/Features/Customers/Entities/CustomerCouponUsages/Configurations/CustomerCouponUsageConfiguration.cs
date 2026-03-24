using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;
using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerCouponUsages.Configurations;

internal sealed class CustomerCouponUsageConfiguration :
    IEntityTypeConfiguration<CustomerCouponUsage>
{
    public void Configure(EntityTypeBuilder<CustomerCouponUsage> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerCouponUsages)
            .HasKey(customerCouponUsage => customerCouponUsage.Id);

        builder
            .Property(customerCouponUsage => customerCouponUsage.Coupon)
            .HasMaxLength(CustomerCouponUsageSettings.CouponMaxLength);

        ConfigureBenefit(builder);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<CustomerCouponUsage> builder)
    {
        builder
            .HasOne(customerCouponUsage => customerCouponUsage.Benefit)
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(customerCouponUsage => customerCouponUsage.BenefitId);
    }
}
