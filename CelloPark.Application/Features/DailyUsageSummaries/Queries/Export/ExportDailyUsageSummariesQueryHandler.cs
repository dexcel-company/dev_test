using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Application.Features.DailyUsageSummaries.Extensions;
using CelloPark.Application.Features.DailyUsageSummaries.Queries.Export.Abstractions;
using CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.DailyUsageSummaries.Queries.Export;

internal sealed class ExportDailyUsageSummariesQueryHandler :
    IExportDailyUsageSummariesQueryHandler
{
    public ExportDailyUsageSummariesQueryHandler(
        IManagementContext manageContext,
        IDailyUsageSummariesExportService dailyUsageSummariesExportService)
    {
        _managementContext = manageContext;
        _dailyUsageSummariesExportService = dailyUsageSummariesExportService;
    }

    private readonly IManagementContext _managementContext;
    private readonly IDailyUsageSummariesExportService _dailyUsageSummariesExportService;

    public async Task<FileStream> HandleAsync(
        ExportDailyUsageSummariesQuery request, CancellationToken cancellationToken = default)
    {
        List<ExportDailyUsageDto> currentItems = await GetDailyItemUsageSummariesAsync(
            request.FilteringCriteria.CurrentStartDate, request.FilteringCriteria.CurrentEndDate, cancellationToken);

        List<ExportDailyUsageDto> referenceItems = await GetDailyItemUsageSummariesAsync(
            request.FilteringCriteria.ReferenceStartDate, request.FilteringCriteria.ReferenceEndDate, cancellationToken);

        List<ExportDailyUsageDto> currentPlans = await GetDailyPlanUsageSummariesAsync(
            request.FilteringCriteria.CurrentStartDate, request.FilteringCriteria.CurrentEndDate, cancellationToken);

        List<ExportDailyUsageDto> referencePlans = await GetDailyPlanUsageSummariesAsync(
            request.FilteringCriteria.ReferenceStartDate, request.FilteringCriteria.ReferenceEndDate, cancellationToken);

        List<ExportDailyUsageDto> currentPackages = await GetDailyPackageUsageSummariesAsync(
            request.FilteringCriteria.CurrentStartDate, request.FilteringCriteria.CurrentEndDate, cancellationToken);

        List<ExportDailyUsageDto> referencePackages = await GetDailyPackageUsageSummariesAsync(
            request.FilteringCriteria.ReferenceStartDate, request.FilteringCriteria.ReferenceEndDate, cancellationToken);

        return _dailyUsageSummariesExportService.Export(
            currentItems: currentItems,
            referenceItems: referenceItems,
            currentPlans: currentPlans,
            referencePlans: referencePlans,
            currentPackages: currentPackages,
            referencePackages: referencePackages,
            currentStartDate: request.FilteringCriteria.CurrentStartDate,
            currentEndDate: request.FilteringCriteria.CurrentEndDate,
            referenceStartDate: request.FilteringCriteria.ReferenceStartDate,
            referenceEndDate: request.FilteringCriteria.ReferenceEndDate);
    }

    private async Task<List<ExportDailyUsageDto>> GetDailyItemUsageSummariesAsync(
        DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyItemUsageSummaries
            .ApplyFilteringByDates(startDate, endDate)
            .GroupBy(dailyItemUsageSummary => dailyItemUsageSummary.Item.Name)
            .Select(group => new ExportDailyUsageDto
            {
                Name = group.Key,
                Quantity = group.Sum(dailyItemUsageSummary => dailyItemUsageSummary.Quantity),
                Gross = group.Sum(dailyItemUsageSummary => dailyItemUsageSummary.Gross),
                Cost = group.Sum(dailyItemUsageSummary => dailyItemUsageSummary.Cost),
                BenefitCost = group.Sum(dailyItemUsageSummary => dailyItemUsageSummary.BenefitCost),
                BenefitQuantity = group.Sum(dailyItemUsageSummary => dailyItemUsageSummary.BenefitQuantity),
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<List<ExportDailyUsageDto>> GetDailyPlanUsageSummariesAsync(
        DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyPlanUsageSummaries
            .ApplyFilteringByDates(startDate, endDate)
            .GroupBy(dailyPlanUsageSummary => dailyPlanUsageSummary.Plan.Name)
            .Select(group => new ExportDailyUsageDto
            {
                Name = group.Key,
                Quantity = group.Sum(dailyPlanUsageSummary => dailyPlanUsageSummary.Quantity),
                Gross = group.Sum(dailyPlanUsageSummary => dailyPlanUsageSummary.Gross),
                Cost = group.Sum(dailyPlanUsageSummary => dailyPlanUsageSummary.Cost),
                BenefitCost = group.Sum(dailyPlanUsageSummary => dailyPlanUsageSummary.BenefitCost),
                BenefitQuantity = group.Sum(dailyPlanUsageSummary => dailyPlanUsageSummary.BenefitQuantity),
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<List<ExportDailyUsageDto>> GetDailyPackageUsageSummariesAsync(
        DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken)
    {
        return await _managementContext.DailyPackageUsageSummaries
            .ApplyFilteringByDates(startDate, endDate)
            .GroupBy(dailyPackageUsageSummary => dailyPackageUsageSummary.Package.Name)
            .Select(group => new ExportDailyUsageDto
            {
                Name = group.Key,
                Quantity = group.Sum(dailyPackageUsageSummary => dailyPackageUsageSummary.Quantity),
                Gross = group.Sum(dailyPackageUsageSummary => dailyPackageUsageSummary.Gross),
                Cost = group.Sum(dailyPackageUsageSummary => dailyPackageUsageSummary.Cost),
                BenefitCost = group.Sum(dailyPackageUsageSummary => dailyPackageUsageSummary.BenefitCost),
                BenefitQuantity = group.Sum(dailyPackageUsageSummary => dailyPackageUsageSummary.BenefitQuantity),
            })
            .ToListAsync(cancellationToken);
    }
}
