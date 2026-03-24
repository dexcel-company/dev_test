using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.DailyPackageUsageCalculations;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyPackageUsageCalculations.Configurations;

internal sealed class DailyPackageUsageCalculationConfiguration :
    IEntityTypeConfiguration<DailyPackageUsageCalculation>
{
    public void Configure(EntityTypeBuilder<DailyPackageUsageCalculation> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyPackageUsageCalculations)
            .HasKey(dailyPackageUsageCalculation => dailyPackageUsageCalculation.Id);

        builder
            .Property(dailyPackageUsageCalculation => dailyPackageUsageCalculation.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyPackageUsageCalculation => dailyPackageUsageCalculation.Cost)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomer(builder);

        ConfigureCustomerPackage(builder);

        ConfigureBenefit(builder);
    }

    private static void ConfigureCustomer(EntityTypeBuilder<DailyPackageUsageCalculation> builder)
    {
        builder
            .HasOne<Customer>()
            .WithMany()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(dailyPackageUsageCalculation => dailyPackageUsageCalculation.CustomerId);
    }

    private static void ConfigureCustomerPackage(EntityTypeBuilder<DailyPackageUsageCalculation> builder)
    {
        builder
            .HasOne<CustomerPackage>()
            .WithMany()
            .HasPrincipalKey(CustomerPackage => CustomerPackage.Id)
            .HasForeignKey(dailyPackageUsageCalculation => dailyPackageUsageCalculation.CustomerPackageId);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<DailyPackageUsageCalculation> builder)
    {
        builder
            .HasOne<Benefit>()
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(dailyPackageUsageCalculation => dailyPackageUsageCalculation.BenefitId);
    }
}
