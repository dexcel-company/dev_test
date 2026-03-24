using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Items;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Packets;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos.Plans;
using CelloPark.Application.Features.DailyUsageSummaries.Extensions;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Plans.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.GetAll;

internal sealed class GetAllDailyUsageSummaryQueryHandler :
    IGetAllDailyUsageSummaryQueryHandler
{
    public GetAllDailyUsageSummaryQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<DailyUsageSummaryPageDto> HandleAsync(
        GetAllDailyUsageSummaryQuery request, CancellationToken cancellationToken = default)
    {
        List<DailyPlanUsageSummaryGroupedPageDto> planSummaries = await GetPlanSummariesAsync(request.FilteringCriteria, cancellationToken);
        List<DailyPackageUsageSummaryGroupedPageDto> packageSummaries = await GetPackageSummariesAsync(request.FilteringCriteria, cancellationToken);
        List<DailyItemUsageSummaryGroupedPageDto> itemSummaries = await GetItemSummariesAsync(request.FilteringCriteria, cancellationToken);

        return new DailyUsageSummaryPageDto
        {
            PlanSummaries = planSummaries,
            PackageSummaries = packageSummaries,
            ItemSummaries = itemSummaries,
        };
    }

    private async Task<List<DailyPlanUsageSummaryGroupedPageDto>> GetPlanSummariesAsync(
        DailyUsageSummaryFilteringQuery filteringCriteria, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyPlanUsageSummaries
            .ApplyFiltering(filteringCriteria)
            .GroupBy(dailyPlanUsageSummary => dailyPlanUsageSummary.Date)
            .Select(group => new DailyPlanUsageSummaryGroupedPageDto
            {
                Date = group.Key,
                Plans = group.Select(dailyPlanUsageSummary => new DailyPlanUsageSummaryPageDto
                {
                    Plan = new PlanLiteDto
                    {
                        Id = dailyPlanUsageSummary.Plan.Id,
                        Name = dailyPlanUsageSummary.Plan.Name,
                    },
                    Gross = dailyPlanUsageSummary.Gross,
                    Cost = dailyPlanUsageSummary.Cost,
                    Quantity = dailyPlanUsageSummary.Quantity,
                    BenefitCost = dailyPlanUsageSummary.BenefitCost,
                    BenefitQuantity = dailyPlanUsageSummary.BenefitQuantity,
                }).ToList(),
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<List<DailyPackageUsageSummaryGroupedPageDto>> GetPackageSummariesAsync(
        DailyUsageSummaryFilteringQuery filteringCriteria, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyPackageUsageSummaries
            .ApplyFiltering(filteringCriteria)
            .GroupBy(dailyPackageUsageSummary => dailyPackageUsageSummary.Date)
            .Select(group => new DailyPackageUsageSummaryGroupedPageDto
            {
                Date = group.Key,
                Packages = group.Select(dailyPackageUsageSummary => new DailyPackageUsageSummaryPageDto
                {
                    Package = new PackageLiteDto
                    {
                        Id = dailyPackageUsageSummary.Package.Id,
                        Name = dailyPackageUsageSummary.Package.Name,
                    },
                    Gross = dailyPackageUsageSummary.Gross,
                    Cost = dailyPackageUsageSummary.Cost,
                    Quantity = dailyPackageUsageSummary.Quantity,
                    BenefitCost = dailyPackageUsageSummary.BenefitCost,
                    BenefitQuantity = dailyPackageUsageSummary.BenefitQuantity,
                }).ToList(),

            })
            .ToListAsync(cancellationToken);
    }

    private async Task<List<DailyItemUsageSummaryGroupedPageDto>> GetItemSummariesAsync(
        DailyUsageSummaryFilteringQuery filteringCriteria, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyItemUsageSummaries
            .ApplyFiltering(filteringCriteria)
            .GroupBy(dailyItemUsageSummary => dailyItemUsageSummary.Date)
            .Select(group => new DailyItemUsageSummaryGroupedPageDto
            {
                Date = group.Key,
                Items = group.Select(dailyItemUsageSummary => new DailyItemUsageSummaryPageDto
                {
                    Item = new ItemLiteDto
                    {
                        Id = dailyItemUsageSummary.Item.Id,
                        Name = dailyItemUsageSummary.Item.Name,
                    },
                    Gross = dailyItemUsageSummary.Gross,
                    Cost = dailyItemUsageSummary.Cost,
                    Quantity = dailyItemUsageSummary.Quantity,
                    BenefitCost = dailyItemUsageSummary.BenefitCost,
                    BenefitQuantity = dailyItemUsageSummary.BenefitQuantity,
                }).ToList(),

            })
            .ToListAsync(cancellationToken);
    }
}
