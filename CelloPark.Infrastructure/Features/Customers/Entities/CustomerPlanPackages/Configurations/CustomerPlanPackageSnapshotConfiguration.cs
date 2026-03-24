using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerPackages.Configurations;

internal sealed class CustomerPackageSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerPackageSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerPackageSnapshot> builder)
    {
        builder
            .ToTable("CustomerPackageSnapshot")
            .HasKey(CustomerPackage => CustomerPackage.Id);

        builder
            .Property(CustomerPackage => CustomerPackage.Id)
            .ValueGeneratedNever();

        builder
            .Property(CustomerPackage => CustomerPackage.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomerCar(builder);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<CustomerPackageSnapshot> builder)
    {
        builder
            .HasOne(CustomerPackage => CustomerPackage.CustomerCar)
            .WithMany()
            .HasPrincipalKey(customerCar => customerCar.Id)
            .HasForeignKey(CustomerPackage => CustomerPackage.CustomerCarId);
    }
}
