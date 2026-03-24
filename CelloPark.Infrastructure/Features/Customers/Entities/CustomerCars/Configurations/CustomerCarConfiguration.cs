using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars.Constants;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerCars.Configurations;

internal sealed class CustomerCarConfiguration :
    IEntityTypeConfiguration<CustomerCar>
{
    public void Configure(EntityTypeBuilder<CustomerCar> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerCars)
            .HasKey(customerCar => customerCar.Id);

        builder
            .Property(customerCar => customerCar.Id)
            .ValueGeneratedNever();

        builder
            .Property(customerCar => customerCar.Number)
            .HasMaxLength(CustomerCarSettings.NumberMaxLength)
            .HasColumnName(DatabaseContextColumnNames.Number);
    }
}
