using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Packets.Configurations;

internal sealed class PackageConfiguration :
    IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Packages)
            .HasKey(package => package.Id);

        builder
            .Property(package => package.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(package => package.Name)
            .HasMaxLength(PackageSettings.NameMaxLength);

        builder
            .Property(package => package.Description)
            .HasMaxLength(PackageSettings.DescriptionMaxLength);

        builder
            .Property(package => package.ContractType)
            .HasConversion(DatabaseContextConverters.ContractTypeConverter);

        ConfigureCreateDetails(builder);

        ConfigureUpdateDetails(builder);

        ConfigureDeleteDetails(builder);

        ConfigurePlanPackage(builder);

        ConfiguraCustomerPackage(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<Package> builder)
    {
        builder
            .OwnsOne(package => package.CreateDetails, buildAction =>
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

    private static void ConfigureUpdateDetails(EntityTypeBuilder<Package> builder)
    {
        builder
            .OwnsOne(package => package.UpdateDetails, buildAction =>
            {
                buildAction
                    .Property(updateDetails => updateDetails.UpdatedAt)
                    .HasConversion(DatabaseContextConverters.DateTimeConverter)
                    .HasColumnName(DatabaseContextColumnNames.UpdatedAt);

                buildAction
                    .Property(updateDetails => updateDetails.UpdatedBy)
                    .HasColumnName(DatabaseContextColumnNames.UpdatedBy);

                buildAction
                    .HasOne(updateDetails => updateDetails.User)
                    .WithMany()
                    .HasPrincipalKey(user => user.Id)
                    .HasForeignKey(updateDetails => updateDetails.UpdatedBy);
            });
    }

    private static void ConfigureDeleteDetails(EntityTypeBuilder<Package> builder)
    {
        builder
            .OwnsOne(package => package.DeleteDetails, buildAction =>
            {
                buildAction
                    .Property(deleteDetails => deleteDetails.DeletedAt)
                    .HasConversion(DatabaseContextConverters.DateTimeConverter)
                    .HasColumnName(DatabaseContextColumnNames.DeletedAt);

                buildAction
                    .Property(deleteDetails => deleteDetails.DeletedBy)
                    .HasColumnName(DatabaseContextColumnNames.DeletedBy);

                buildAction
                    .HasOne(deleteDetails => deleteDetails.User)
                    .WithMany()
                    .HasPrincipalKey(user => user.Id)
                    .HasForeignKey(deleteDetails => deleteDetails.DeletedBy);
            });
    }

    private static void ConfigurePlanPackage(EntityTypeBuilder<Package> builder)
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

    private static void ConfiguraCustomerPackage(EntityTypeBuilder<Package> builder)
    {
        builder
            .HasMany<CustomerPackage>()
            .WithOne(CustomerPackage => CustomerPackage.Package)
            .HasPrincipalKey(package => package.Id)
            .HasForeignKey(CustomerPackage => CustomerPackage.PackageId);
    }
}
