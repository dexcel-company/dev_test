using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerDailyCharges;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerDailyCharges.Configurations;

internal class CustomerDailyChargeSnapshotConfiguration :
    IEntityTypeConfiguration<CustomerDailyChargeSnapshot>
{
    public void Configure(EntityTypeBuilder<CustomerDailyChargeSnapshot> builder)
    {
        builder
            .ToTable("DailyChargeSnapshot")
            .HasKey(dailyCharge => dailyCharge.Id);

        builder
            .Property(dailyCharge => dailyCharge.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyCharge => dailyCharge.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        ConfigureCustomerCar(builder);

        ConfigureCustomerItem(builder);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<CustomerDailyChargeSnapshot> builder)
    {
        builder
            .HasOne<CustomerCar>()
            .WithMany()
            .HasPrincipalKey(customerCar => customerCar.Id)
            .HasForeignKey(dailyCharge => dailyCharge.CustomerCarId);
    }

    private static void ConfigureCustomerItem(EntityTypeBuilder<CustomerDailyChargeSnapshot> builder)
    {
        builder
            .HasOne(dailyCharge => dailyCharge.Item)
            .WithMany()
            .HasPrincipalKey(item => item.Id)
            .HasForeignKey(dailyCharge => dailyCharge.ItemId);
    }
}
