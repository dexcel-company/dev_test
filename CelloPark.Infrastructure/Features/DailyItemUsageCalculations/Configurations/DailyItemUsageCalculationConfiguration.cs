using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.DailyItemUsageCalculations;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyItemUsageCalculations.Configurations;

internal sealed class DailyItemUsageCalculationConfiguration :
    IEntityTypeConfiguration<DailyItemUsageCalculation>
{
    public void Configure(EntityTypeBuilder<DailyItemUsageCalculation> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyItemUsageCalculations)
            .HasKey(dailyItemUsageCalculation => dailyItemUsageCalculation.Id);

        builder
            .Property(dailyItemUsageCalculation => dailyItemUsageCalculation.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyItemUsageCalculation => dailyItemUsageCalculation.Cost)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomer(builder);

        ConfigureCustomerCar(builder);

        ConfigureBenefit(builder);
    }

    private static void ConfigureCustomer(EntityTypeBuilder<DailyItemUsageCalculation> builder)
    {
        builder
            .HasOne<Customer>()
            .WithMany()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(dailyItemUsageCalculation => dailyItemUsageCalculation.CustomerId);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<DailyItemUsageCalculation> builder)
    {
        builder
            .HasOne<CustomerCar>()
            .WithMany()
            .HasPrincipalKey(CustomerPackage => CustomerPackage.Id)
            .HasForeignKey(dailyItemUsageCalculation => dailyItemUsageCalculation.CustomerCarId);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<DailyItemUsageCalculation> builder)
    {
        builder
            .HasOne<Benefit>()
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(dailyItemUsageCalculation => dailyItemUsageCalculation.BenefitId);
    }
}
