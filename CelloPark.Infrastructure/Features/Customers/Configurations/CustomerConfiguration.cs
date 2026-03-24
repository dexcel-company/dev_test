using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Constants;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Configurations;

internal sealed class CustomerConfiguration :
    IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Customers)
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

        ConfigureCreateDetails(builder);

        ConfigureCustomerPlan(builder);

        ConfigureCustomerCar(builder);

        ConfigureCustomerBenefit(builder);

        ConfigureCustomerCredit(builder);

        ConfigureCustomerCouponUsage(builder);

        ConfigureDailyCharges(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<Customer> builder)
    {
        builder
            .OwnsOne(customer => customer.CreateDetails, buildAction =>
            {
                buildAction
                    .Property(createDetails => createDetails.CreatedAt)
                    .HasConversion(DatabaseContextConverters.DateTimeConverter)
                    .HasColumnName(DatabaseContextColumnNames.CreatedAt);

                buildAction
                    .Property(createDetails => createDetails.CreatedBy)
                    .HasColumnName(DatabaseContextColumnNames.CreatedBy);

                buildAction
                    .HasOne(createDetails => createDetails.User)
                    .WithMany()
                    .HasPrincipalKey(user => user.Id)
                    .HasForeignKey(createDetails => createDetails.CreatedBy);
            });
    }

    private static void ConfigureCustomerPlan(EntityTypeBuilder<Customer> builder)
    {
        builder
            .HasOne(customer => customer.Plan)
            .WithOne()
            .HasPrincipalKey<CustomerPlan>(customerPlan => customerPlan.Id)
            .HasForeignKey<Customer>(customer => customer.CustomerPlanId);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<Customer> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Customer.Cars))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Cars)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCar => customerCar.CustomerId);
    }

    private static void ConfigureCustomerBenefit(EntityTypeBuilder<Customer> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Customer.Benefits))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Benefits)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerBenefit => customerBenefit.CustomerId);
    }

    private static void ConfigureCustomerCredit(EntityTypeBuilder<Customer> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Customer.Credits))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.Credits)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCredit => customerCredit.CustomerId);
    }

    private static void ConfigureCustomerCouponUsage(EntityTypeBuilder<Customer> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Customer.CouponUsages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.CouponUsages)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(customerCouponUsage => customerCouponUsage.CustomerId);
    }

    private static void ConfigureDailyCharges(EntityTypeBuilder<Customer> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Customer.DailyCharges))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customer => customer.DailyCharges)
            .WithOne()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(dailyCharge => dailyCharge.CustomerId);
    }
}
