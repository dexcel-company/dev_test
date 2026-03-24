using CelloPark.Domain.Features.Customers.Entities.CustomerCredits;
using CelloPark.Domain.Features.Customers.Entities.CustomerCredits.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerCredits.Configurations;

internal sealed class CustomerCreditSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerCreditSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerCreditSnapshot> builder)
    {
        builder
            .ToTable("CustomerCreditSnapshot")
            .HasKey(customerCredit => customerCredit.Id);

        builder
            .Property(customerCredit => customerCredit.Id)
            .ValueGeneratedNever();

        builder
            .Property(customerCredit => customerCredit.Description)
            .HasMaxLength(CustomerCreditSettings.DescriptionMaxLength);

        builder
            .Property(customerCredit => customerCredit.Balance)
            .HasColumnType(DatabaseContextColumnTypes.Price);
    }
}
