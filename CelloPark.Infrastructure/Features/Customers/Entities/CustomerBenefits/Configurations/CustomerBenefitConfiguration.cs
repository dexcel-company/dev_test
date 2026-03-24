using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerBenefits.Configurations;

internal sealed class CustomerBenefitConfiguration :
    IEntityTypeConfiguration<CustomerBenefit>
{
    public void Configure(EntityTypeBuilder<CustomerBenefit> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerBenefits)
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

        builder
            .Property(customerBenefit => customerBenefit.StartDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        builder
            .Property(CustomerPackage => CustomerPackage.EndDate)
            .HasConversion(DatabaseContextConverters.DateTimeConverter);

        ConfigureBenefit(builder);
    }

    private static void ConfigureBenefit(EntityTypeBuilder<CustomerBenefit> builder)
    {
        builder
            .HasOne(customerBenefit => customerBenefit.Benefit)
            .WithMany()
            .HasPrincipalKey(benefit => benefit.Id)
            .HasForeignKey(customerBenefit => customerBenefit.BenefitId);
    }
}
