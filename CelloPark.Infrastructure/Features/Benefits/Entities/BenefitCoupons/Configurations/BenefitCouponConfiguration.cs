using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Benefits.Entities.BenefitCoupons.Configurations;

internal sealed class BenefitCouponConfiguration :
    IEntityTypeConfiguration<BenefitCoupon>
{
    public void Configure(EntityTypeBuilder<BenefitCoupon> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.BenefitCoupons)
            .HasKey(benefitCoupon => benefitCoupon.Id);

        builder
            .Property(benefitCoupon => benefitCoupon.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(benefitCoupon => benefitCoupon.Coupon)
            .HasMaxLength(20);

        builder
            .Property(benefitCoupon => benefitCoupon.CouponType)
            .HasConversion(DatabaseContextConverters.CouponTypeConverter);

        ConfigureCreateDetails(builder);

        ConfigureUpdateDetails(builder);

        ConfigureDeleteDetails(builder);
    }

    private static void ConfigureCreateDetails(EntityTypeBuilder<BenefitCoupon> builder)
    {
        builder
            .OwnsOne(benefitCoupon => benefitCoupon.CreateDetails, buildAction =>
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

    private static void ConfigureUpdateDetails(EntityTypeBuilder<BenefitCoupon> builder)
    {
        builder
            .OwnsOne(benefitCoupon => benefitCoupon.UpdateDetails, buildAction =>
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

    private static void ConfigureDeleteDetails(EntityTypeBuilder<BenefitCoupon> builder)
    {
        builder
            .OwnsOne(benefitCoupon => benefitCoupon.DeleteDetails, buildAction =>
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
