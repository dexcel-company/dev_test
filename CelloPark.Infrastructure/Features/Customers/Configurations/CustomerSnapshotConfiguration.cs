using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Constants;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Configurations;

internal sealed class CustomerSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder
            .ToTable("CustomerSnapshot")
            .HasKey(customer => customer.Id);

        builder
            .Property(customer => customer.Id)
            .ValueGeneratedNever();

        builder
            .Property(customer => customer.ShadowId)
            .HasMaxLength(CustomerSettings.ShadowIdMaxlength);

        builder
            .Property(customer => customer.Name)
            .HasMaxLength(CustomerSettings.NameMaxLength);

        builder
            .Property(customer => customer.ContractType)
            .HasConversion(DatabaseContextConverters.ContractTypeConverter);

        ConfigureCustomerPlan(builder);

        ConfigureCustomerCar(builder);

        ConfigureCustomerBenefit(builder);

        ConfigureCustomerCredit(builder);

        ConfigureCustomerCouponUsage(builder);

        ConfigureDailyCharges(builder);
    }

    private static void ConfigureCustomerPlan(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder
            .HasOne(customer => customer.Plan)
            .WithOne()
            .HasPrincipalKey<CustomerPlanSnapshot>(customerPlan => customerPlan.Id)
            .HasForeignKey<CustomerSnapshot>(customer => customer.CustomerPlanId);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerSnapshot.Cars))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Cars)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCar => customerCar.CustomerId);
    }

    private static void ConfigureCustomerBenefit(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerSnapshot.Benefits))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Benefits)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerBenefit => customerBenefit.CustomerId);
    }

    private static void ConfigureCustomerCredit(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerSnapshot.Credits))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Credits)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCredit => customerCredit.CustomerId);
    }

    private static void ConfigureCustomerCouponUsage(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerSnapshot.CouponUsages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.CouponUsages)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCouponUsage => customerCouponUsage.CustomerId);
    }

    private static void ConfigureDailyCharges(EntityTypeBuilder<CustomerSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerSnapshot.DailyCharges))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.DailyCharges)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(dailyCharge => dailyCharge.CustomerId);
    }
}
