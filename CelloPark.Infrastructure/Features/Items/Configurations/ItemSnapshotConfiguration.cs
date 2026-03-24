using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Items.Constants;
using CelloPark.Infrastructure.Common.Contexts.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.Items.Configurations;

internal sealed class ItemSnapshotConfiguration :
    IEntityTypeConfiguration<ItemSnapshot>
{
    public void Configure(EntityTypeBuilder<ItemSnapshot> builder)
    {
        builder
            .ToTable("ItemSnapshot")
            .HasKey(item => item.Id);

        builder
            .Property(item => item.Id)
            .ValueGeneratedNever();

        builder
            .Property(item => item.Name)
            .HasMaxLength(ItemSettings.NameMaxLength);

        builder
            .Property(item => item.ContractType)
            .HasConversion(DatabaseContextConverters.ContractTypeConverter);
    }
}
