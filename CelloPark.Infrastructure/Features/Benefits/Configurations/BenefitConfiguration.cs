using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Benefits.Configurations;

internal sealed class BenefitConfiguration :
    IEntityTypeConfiguration<Benefit>
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.Benefits)
            .HasKey(benefit => benefit.Id);

        builder
            .Property(benefit => benefit.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(benefit => benefit.Name)
            .HasMaxLength(BenefitSettings.NameMaxLength);

        builder
            .Property(benefit => benefit.Description)
            .HasMaxLength(BenefitSettings.DescriptionMaxLength);

        builder
            .Property(benefit => benefit.StartActiveDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        builder
            .Property(benefit => benefit.EndActiveDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        builder
            .Property(benefit => benefit.StartPromotionDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        builder
            .Property(benefit => benefit.EndPromotionDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        ConfigureCreateDetails(builder);

        ConfigureUpdateDetails(builder);

        ConfigureDeleteDetails(builder);

        ConfigureBenefitCoupon(builder);

        ConfigureBenfitPaymentCategory(builder);

        ConfigureIndexes(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<Benefit> builder)
    {
        builder
            .OwnsOne(benefit => benefit.CreateDetails, buildAction =>
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

    private static void ConfigureUpdateDetails(EntityTypeBuilder<Benefit> builder)
    {
        builder
            .OwnsOne(benefit => benefit.UpdateDetails, buildAction =>
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

    private static void ConfigureDeleteDetails(EntityTypeBuilder<Benefit> builder)
    {
        builder
            .OwnsOne(benefit => benefit.DeleteDetails, buildAction =>
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

    private static void ConfigureBenefitCoupon(EntityTypeBuilder<Benefit> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Benefit.Coupons))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(benefit => benefit.Coupons)
            .WithOne(benefitCoupon => benefitCoupon.Benefit)
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(benefitCoupon => benefitCoupon.BenefitId);
    }

    private static void ConfigureBenfitPaymentCategory(EntityTypeBuilder<Benefit> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(Benefit.PaymentCategories))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(benefit => benefit.PaymentCategories)
            .WithOne()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(benefitPaymentCategory => benefitPaymentCategory.BenefitId);
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Benefit> builder)
    {
        builder
            .HasIndex(benefit => benefit.Name)
            .IsUnique();
    }
}
