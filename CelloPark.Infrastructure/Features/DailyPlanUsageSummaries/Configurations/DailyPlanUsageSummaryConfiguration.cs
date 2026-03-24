using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using CelloPark.Infrastructure.Common.Contexts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CelloPark.Infrastructure.Features.DailyPlanUsageSummaries.Configurations;

internal sealed class DailyPlanUsageSummaryConfiguration :
    IEntityTypeConfiguration<DailyPlanUsageSummary>
{
    public void Configure(EntityTypeBuilder<DailyPlanUsageSummary> builder)
    {
        builder
            .ToTable(DatabaseContextTableNames.DailyPlanUsageSummaries)
            .HasKey(dailyPlanUsageSummary => dailyPlanUsageSummary.Id);

        builder
            .Property(dailyPlanUsageSummary => dailyPlanUsageSummary.Id)
            .ValueGeneratedNever();

        builder
            .Property(dailyPlanUsageSummary => dailyPlanUsageSummary.Date)
            .HasColumnType("DATE");

        builder
            .Property(dailyPlanUsageSummary => dailyPlanUsageSummary.Gross)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyPlanUsageSummary => dailyPlanUsageSummary.Cost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        builder
            .Property(dailyPlanUsageSummary => dailyPlanUsageSummary.BenefitCost)
            .HasColumnType(DatabaseContextColumnTypes.LargePrice);

        ConfigurePlan(builder);
    }

    private static void ConfigurePlan(EntityTypeBuilder<DailyPlanUsageSummary> builder)
    {
        builder
            .HasOne(dailyPlanUsageSummary => dailyPlanUsageSummary.Plan)
            .WithMany()
            .HasPrincipalKey(plan => plan.Id)
            .HasForeignKey(dailyPlanUsageSummary => dailyPlanUsageSummary.PlanId);
    }
}
