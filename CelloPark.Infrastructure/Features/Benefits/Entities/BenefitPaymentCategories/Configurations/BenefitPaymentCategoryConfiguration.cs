using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Benefits.Entities.BenefitPaymentCategories.Configurations;

internal sealed class BenefitPaymentCategoryConfiguration :
    IEntityTypeConfiguration<BenefitPaymentCategory>
{
    public void Configure(EntityTypeBuilder<BenefitPaymentCategory> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.BenefitPaymentCategories)
            .HasKey(benefitPaymentCategory => benefitPaymentCategory.Id);

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.Amount)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.AmountLimit)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.AmountType)
            .HasConversion(DatabaseContextConverters.AmountTypeConverter);

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.FrequencyType)
            .HasConversion(DatabaseContextConverters.FrequencyTypeConverter);

        ConfigureCreateDetails(builder);

        ConfigureUpdateDetails(builder);

        ConfigureDeleteDetails(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<BenefitPaymentCategory> builder)
    {
        builder
            .OwnsOne(benefitPaymentCategory => benefitPaymentCategory.CreateDetails, buildAction =>
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

    private static void ConfigureUpdateDetails(EntityTypeBuilder<BenefitPaymentCategory> builder)
    {
        builder
            .OwnsOne(benefitPaymentCategory => benefitPaymentCategory.UpdateDetails, buildAction =>
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

    private static void ConfigureDeleteDetails(EntityTypeBuilder<BenefitPaymentCategory> builder)
    {
        builder
            .OwnsOne(benefitPaymentCategory => benefitPaymentCategory.DeleteDetails, buildAction =>
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
