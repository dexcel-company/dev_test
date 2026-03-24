using CelloPark.Domain.Features.Customers.Entities.CustomerCredits;
using CelloPark.Domain.Features.Customers.Entities.CustomerCredits.Constants;
using CelloPark.Domain.Features.Items;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerCredits.Configurations;

internal sealed class CustomerCreditConfiguration :
    IEntityTypeConfiguration<CustomerCredit>
{
    public void Configure(EntityTypeBuilder<CustomerCredit> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerCredits)
            .HasKey(customerCredit => customerCredit.Id);

        builder
            .Property(customerCredit => customerCredit.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();

        builder
            .Property(customerCredit => customerCredit.Description)
            .HasMaxLength(CustomerCreditSettings.DescriptionMaxLength);

        builder
            .Property(customerCredit => customerCredit.Type)
            .HasMaxLength(CustomerCreditSettings.TypeMaxLength);

        builder
            .Property(customerCredit => customerCredit.Balance)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureItem(builder);
    }

    private static void ConfigureItem(EntityTypeBuilder<CustomerCredit> builder)
    {
        builder
            .HasOne(customerCredit => customerCredit.Item)
            .WithOne()
            .HasPrincipalKey<Item>(item => item.Id)
            .HasForeignKey<CustomerCredit>(customerCredit => customerCredit.ItemId);
    }
}
