using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Benefits.Entities.BenefitPaymentCategories.Configurations;

internal sealed class BenefitPaymentCategorySnapshotConfiguration :
    IEntityTypeConfiguration<BenefitPaymentCategorySnapshot>
{
    public void Configure(EntityTypeBuilder<BenefitPaymentCategorySnapshot> builder)
    {
        builder
            .ToTable("BenefitPaymentCategorySnapshot")
            .HasKey(benefitPaymentCategory => benefitPaymentCategory.Id);

        builder
            .Property(benefitPaymentCategory => benefitPaymentCategory.Id)
            .ValueGeneratedNever();

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
    }
}
