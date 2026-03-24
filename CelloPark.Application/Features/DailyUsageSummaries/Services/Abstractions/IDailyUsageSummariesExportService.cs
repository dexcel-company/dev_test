using CelloPark.Application.Features.DailyUsageSummaries.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;

public interface IDailyUsageSummariesExportService
{
    FileStream Export(
        List<ExportDailyUsageDto> currentItems,
        List<ExportDailyUsageDto> referenceItems,
        List<ExportDailyUsageDto> currentPlans,
        List<ExportDailyUsageDto> referencePlans,
        List<ExportDailyUsageDto> currentPackages,
        List<ExportDailyUsageDto> referencePackages,
        DateOnly? currentStartDate,
        DateOnly? currentEndDate,
        DateOnly? referenceStartDate,
        DateOnly? referenceEndDate);
}
