using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyItemUsageSummaries.Configurations;

internal sealed class DailyItemUsageSummaryConfiguration :
    IEntityTypeConfiguration<DailyItemUsageSummary>
{
    public void Configure(EntityTypeBuilder<DailyItemUsageSummary> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyItemUsageSummaries)
            .HasKey(dailyItemUsageSummary => dailyItemUsageSummary.Id);

        builder
            .Property(dailyItemUsageSummary => dailyItemUsageSummary.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyItemUsageSummary => dailyItemUsageSummary.Date)
            .HasColumnType("DATE");

        builder
            .Property(dailyItemUsageSummary => dailyItemUsageSummary.Gross)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyItemUsageSummary => dailyItemUsageSummary.Cost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyItemUsageSummary => dailyItemUsageSummary.BenefitCost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        ConfigureItem(builder);
    }

    private static void ConfigureItem(EntityTypeBuilder<DailyItemUsageSummary> builder)
    {
        builder
            .HasOne(dailyItemUsageSummary => dailyItemUsageSummary.Item)
            .WithMany()
            .HasPrincipalKey(item => item.Id)
            .HasForeignKey(dailyItemUsageSummary => dailyItemUsageSummary.ItemId);
    }
}
