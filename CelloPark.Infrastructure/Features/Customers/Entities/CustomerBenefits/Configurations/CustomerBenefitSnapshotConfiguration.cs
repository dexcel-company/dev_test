using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerBenefits.Configurations;

internal sealed class CustomerBenefitSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerBenefitSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerBenefitSnapshot> builder)
    {
        builder
            .ToTable("CustomerBenefitSnapshot")
            .HasKey(customerBenefit => customerBenefit.Id);

        builder
            .Property(customerBenefit => customerBenefit.Id)
            .ValueGeneratedNever();

        builder
            .Property(customerBenefit => customerBenefit.Debit)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        builder
            .Property(customerBenefit => customerBenefit.LimitAmountLeft)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureBenefit(builder);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<CustomerBenefitSnapshot> builder)
    {
        builder
            .HasOne(customerBenefit => customerBenefit.Benefit)
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(customerBenefit => customerBenefit.BenefitId);
    }
}
