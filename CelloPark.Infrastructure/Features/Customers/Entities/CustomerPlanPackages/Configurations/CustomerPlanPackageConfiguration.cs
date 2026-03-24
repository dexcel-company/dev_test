using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerPackages.Configurations;

internal sealed class CustomerPackageConfiguration :
    IEntityTypeConfiguration<CustomerPackage>
{
    public void Configure(EntityTypeBuilder<CustomerPackage> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.CustomerPackages)
            .HasKey(CustomerPackage => CustomerPackage.Id);

        builder
            .Property(CustomerPackage => CustomerPackage.Id)
            .ValueGeneratedNever();

        builder
            .Property(CustomerPackage => CustomerPackage.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomerCar(builder);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<CustomerPackage> builder)
    {
        builder
            .HasOne(CustomerPackage => CustomerPackage.CustomerCar)
            .WithMany()
            .HasPrincipalKey(customerCar => customerCar.Id)
            .HasForeignKey(CustomerPackage => CustomerPackage.CustomerCarId);
    }
}
