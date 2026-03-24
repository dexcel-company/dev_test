using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Packets.Configurations;

internal sealed class PackageSnapshotConfiguration :
    IEntityTypeConfiguration<PackageSnapshot>
{
    public void Configure(EntityTypeBuilder<PackageSnapshot> builder)
    {
        builder
            .ToTable("PackageSnapshot")
            .HasKey(package => package.Id);

        builder
            .Property(package => package.Id)
            .ValueGeneratedNever();

        builder
            .Property(package => package.Name)
            .HasMaxLength(PackageSettings.NameMaxLength);

        builder
            .Property(package => package.ContractType)
            .HasConversion(DatabaseContextConverters.ContractTypeConverter);

        ConfigurePlanPackage(builder);

        ConfiguraCustomerPackage(builder);
    }

    private static void ConfigurePlanPackage(EntityTypeBuilder<PackageSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Package.PlanPackages))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        //builder
        //    .HasMany(package => package.PlanPackages)
        //    .WithOne(planPackage => planPackage.Package)
        //    .HasPrincipalKey(package => package.Id)
        //    .HasForeignKey(planPackage => planPackage.PackageId);
    }

    private static void ConfiguraCustomerPackage(EntityTypeBuilder<PackageSnapshot> builder)
    {
        builder
            .HasMany<CustomerPackageSnapshot>()
            .WithOne(CustomerPackage => CustomerPackage.Package)
            .HasPrincipalKey(package => package.Id)
            .HasForeignKey(CustomerPackage => CustomerPackage.PackageId);
    }
}
