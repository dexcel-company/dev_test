using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.PlanPackages.Configurations;

internal sealed class PlanPackageSnapshotConfiguration :
    IEntityTypeConfiguration<PlanPackageSnapshot>
{
    public void Configure(EntityTypeBuilder<PlanPackageSnapshot> builder)
    {
        builder
            .ToTable("PlanPackageSnapshot")
            .HasKey(planPackage => planPackage.Id);

        builder
            .Property(planPackage => planPackage.Id)
            .ValueGeneratedNever();

        builder
            .Property(planPackage => planPackage.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        // TODO move to other configs

        builder
            .HasOne(x => x.Plan)
            .WithMany(x => x.PlanPackages)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.PlanId);

        builder
            .HasOne(x => x.Package)
            .WithMany(x => x.PlanPackages)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.PackageId);
    }
}
