using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerDailyCharges;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Customers.Entities.CustomerDailyCharges.Configurations;

internal sealed class CustomerDailyChargeConfiguration :
    IEntityTypeConfiguration<CustomerDailyCharge>
{
    public void Configure(EntityTypeBuilder<CustomerDailyCharge> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyCharges)
            .HasKey(dailyCharge => dailyCharge.Id);

        builder
            .Property(dailyCharge => dailyCharge.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyCharge => dailyCharge.Price)
            .HasColumnType(DatabaseContextColumnTypes.Price);

        builder
            .Property(dailyCharge => dailyCharge.Count)
            .HasColumnName("Count");

        ConfigureCustomerCar(builder);

        ConfigureCustomerItem(builder);
    }

    private static void ConfigureCustomerCar(EntityTypeBuilder<CustomerDailyCharge> builder)
    {
        builder
            .HasOne<CustomerCar>()
            .WithMany()
            .HasPrincipalKey(customerCar => customerCar.Id)
            .HasForeignKey(dailyCharge => dailyCharge.CustomerCarId);
    }

    private static void ConfigureCustomerItem(EntityTypeBuilder<CustomerDailyCharge> builder)
    {
        builder
            .HasOne(dailyCharge => dailyCharge.Item)
            .WithMany()
            .HasPrincipalKey(item => item.Id)
            .HasForeignKey(dailyCharge => dailyCharge.ItemId);
    }
}
