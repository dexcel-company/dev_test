using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Plans.Configurations;

internal sealed class PlanConfiguration :
    IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Plans)
            .HasKey(plan => plan.Id);

        builder
            .Property(plan => plan.Name)
            .HasMaxLength(PlanSettings.NameMaxLength);

        builder
            .Property(plan => plan.Description)
            .HasMaxLength(PlanSettings.DescriptionMaxLength);

        builder
            .Property(plan => plan.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        builder
            .Property(plan => plan.ContractType)
            .HasConversion(DatabaseContextConverters.ContractTypeConverter);

        builder
            .Property(plan => plan.CalculationType)
            .HasConversion(DatabaseContextConverters.CalculationTypeConverter);

        ConfigurePlanPackage(builder);
    }

    private static void ConfigurePlanPackage(EntityTypeBuilder<Plan> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Plan.PlanPackages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        //builder
        //    .HasMany(plan => plan.PlanPackages)
        //    .WithOne(planPackage => planPackage.Plan)
        //    .HasPrincipalKey(plan => plan.Id)
        //    .HasForeignKey(planPackage => planPackage.PackageId);
    }
}
