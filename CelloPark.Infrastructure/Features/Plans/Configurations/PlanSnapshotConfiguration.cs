using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Plans.Configurations;

internal sealed class PlanSnapshotConfiguration :
    IEntityTypeConfiguration<PlanSnapshot>
{
    public void Configure(EntityTypeBuilder<PlanSnapshot> builder)
    {
        builder
            .ToTable("PlanSnapshot")
            .HasKey(plan => plan.Id);

        builder
            .Property(plan => plan.Id)
            .ValueGeneratedNever();

        builder
            .Property(plan => plan.Name)
            .HasMaxLength(PlanSettings.NameMaxLength);

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

    private static void ConfigurePlanPackage(EntityTypeBuilder<PlanSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(PlanSnapshot.PlanPackages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        //builder
        //    .HasMany(plan => plan.PlanPackages)
        //    .WithOne(planPackage => planPackage.Plan)
        //    .HasPrincipalKey(plan => plan.Id)
        //    .HasForeignKey(planPackage => planPackage.PackageId);
    }
}
