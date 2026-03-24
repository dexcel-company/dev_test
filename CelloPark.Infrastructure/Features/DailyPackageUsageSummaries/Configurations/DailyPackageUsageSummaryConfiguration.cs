using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyPackageUsageSummaries.Configurations;

internal sealed class DailyPackageUsageSummaryConfiguration :
    IEntityTypeConfiguration<DailyPackageUsageSummary>
{
    public void Configure(EntityTypeBuilder<DailyPackageUsageSummary> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyPackageUsageSummaries)
            .HasKey(dailyPackageUsageSummary => dailyPackageUsageSummary.Id);

        builder
            .Property(dailyPackageUsageSummary => dailyPackageUsageSummary.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyPackageUsageSummary => dailyPackageUsageSummary.Date)
            .HasColumnType("DATE");

        builder
            .Property(dailyPackageUsageSummary => dailyPackageUsageSummary.Gross)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyPackageUsageSummary => dailyPackageUsageSummary.Cost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyPackageUsageSummary => dailyPackageUsageSummary.BenefitCost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        ConfigurePackage(builder);
    }

    private static void ConfigurePackage(EntityTypeBuilder<DailyPackageUsageSummary> builder)
    {
        builder
            .HasOne(dailyPackageUsageSummary => dailyPackageUsageSummary.Package)
            .WithMany()
            .HasPrincipalKey(package => package.Id)
            .HasForeignKey(dailyPackageUsageSummary => dailyPackageUsageSummary.PackageId);
    }
}
