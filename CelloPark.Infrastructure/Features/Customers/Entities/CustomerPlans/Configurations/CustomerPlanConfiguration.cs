using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerPlans.Configurations;

internal sealed class CustomerPlanConfiguration :
    IEntityTypeConfiguration<CustomerPlan>
{
    public void Configure(EntityTypeBuilder<CustomerPlan> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerPlans)
            .HasKey(customerPlan => customerPlan.Id);

        builder
            .Property(customerPlan => customerPlan.Id)
            .ValueGeneratedNever();

        builder
            .Property(customerPlan => customerPlan.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigurePlan(builder);

        ConfigureCustomerPackage(builder);
    }

    private static void ConfigurePlan(EntityTypeBuilder<CustomerPlan> builder)
    {
        builder
            .HasOne(customerPlan => customerPlan.Plan)
            .WithMany()
            .HasPrincipalKey(plan => plan.Id)
            .HasForeignKey(customerPlan => customerPlan.PlanId);
    }

    private static void ConfigureCustomerPackage(EntityTypeBuilder<CustomerPlan> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(CustomerPlan.PlanPackages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(customerPlan => customerPlan.PlanPackages)
            .WithOne()
            .HasPrincipalKey(customerPlan => customerPlan.Id)
            .HasForeignKey(CustomerPackage => CustomerPackage.CustomerPlanId);
    }
}
