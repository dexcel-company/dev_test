using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Benefits.Configurations;

internal sealed class BenefitSnapshotConfiguration :
    IEntityTypeConfiguration<BenefitSnapshot>
{
    public void Configure(EntityTypeBuilder<BenefitSnapshot> builder)
    {
        builder
            .ToTable("BenefitSnapshot")
            .HasKey(benefit => benefit.Id);

        builder
            .Property(benefit => benefit.Id)
            .ValueGeneratedNever();

        builder
            .Property(benefit => benefit.Name)
            .HasMaxLength(BenefitSettings.NameMaxLength);

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

        ConfigureBenefitCoupon(builder);

        ConfigureBenfitPaymentCategory(builder);
    }

    private static void ConfigureBenefitCoupon(EntityTypeBuilder<BenefitSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(BenefitSnapshot.Coupons))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(benefit => benefit.Coupons)
            .WithOne(benefitCoupon => benefitCoupon.Benefit)
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(benefitCoupon => benefitCoupon.BenefitId);
    }

    private static void ConfigureBenfitPaymentCategory(EntityTypeBuilder<BenefitSnapshot> builder)
    {
        builder.Metadata
            .FindNavigation(nameof(BenefitSnapshot.PaymentCategories))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasMany(benefit => benefit.PaymentCategories)
            .WithOne()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(benefitPaymentCategory => benefitPaymentCategory.BenefitId);
    }
}
