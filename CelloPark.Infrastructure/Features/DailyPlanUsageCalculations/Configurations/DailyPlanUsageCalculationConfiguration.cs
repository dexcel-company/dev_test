using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.DailyPlanCalculations;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyPlanUsageCalculations.Configurations;

internal sealed class DailyPlanUsageCalculationConfiguration :
    IEntityTypeConfiguration<DailyPlanUsageCalculation>
{
    public void Configure(EntityTypeBuilder<DailyPlanUsageCalculation> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyPlanUsageCalculations)
            .HasKey(dailyPlanUsageCalculation => dailyPlanUsageCalculation.Id);

        builder
            .Property(dailyPlanUsageCalculation => dailyPlanUsageCalculation.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyPlanUsageCalculation => dailyPlanUsageCalculation.Cost)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomer(builder);

        ConfigureCustomerPlan(builder);

        ConfigureBenefit(builder);
    }

    public static void ConfigureCustomer(EntityTypeBuilder<DailyPlanUsageCalculation> builder)
    {
        builder
            .HasOne<Customer>()
            .WithMany()
            .HasPrincipalKey(customer => customer.Id)
            .HasForeignKey(dailyPlanUsageCalculation => dailyPlanUsageCalculation.CustomerId);
    }

    public static void ConfigureCustomerPlan(EntityTypeBuilder<DailyPlanUsageCalculation> builder)
    {
        builder
            .HasOne<CustomerPlan>()
            .WithMany()
            .HasPrincipalKey(customerPlan => customerPlan.Id)
            .HasForeignKey(dailyPlanUsageCalculation => dailyPlanUsageCalculation.CustomerPlanId);
    }

    public static void ConfigureBenefit(EntityTypeBuilder<DailyPlanUsageCalculation> builder)
    {
        builder
            .HasOne<Benefit>()
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(dailyPlanUsageCalculation => dailyPlanUsageCalculation.BenefitId);
    }
}
