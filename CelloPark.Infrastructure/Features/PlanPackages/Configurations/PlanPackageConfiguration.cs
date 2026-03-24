using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.PlanPackages.Configurations;

internal sealed class PlanPackageConfiguration :
    IEntityTypeConfiguration<PlanPackage>
{
    public void Configure(EntityTypeBuilder<PlanPackage> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.PlanPackages)
            .HasKey(planPackage => new { planPackage.PlanId, planPackage.PackageId });

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

        ConfigureCreateDetails(builder);

        ConfigureUpdateDetails(builder);

        ConfigureDeleteDetails(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<PlanPackage> builder)
    {
        builder
            .OwnsOne(planPackage => planPackage.CreateDetails, buildAction =>
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

    private static void ConfigureUpdateDetails(EntityTypeBuilder<PlanPackage> builder)
    {
        builder
            .OwnsOne(planPackage => planPackage.UpdateDetails, buildAction =>
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

    private static void ConfigureDeleteDetails(EntityTypeBuilder<PlanPackage> builder)
    {
        builder
            .OwnsOne(planPackage => planPackage.DeleteDetails, buildAction =>
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
}
