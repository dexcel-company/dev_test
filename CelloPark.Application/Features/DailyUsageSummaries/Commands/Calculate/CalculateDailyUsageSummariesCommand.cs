using CelloPark.Application.Features.DailyUsageSummaries.Dtos;

namespace CelloPark.Application.Features.DailyUsageSummaries.Commands.Calculate;

public sealed class CalculateDailyUsageSummariesCommand
{
    public CalculateDailyUsageSummariesCommand(DailyUsageSummaryCalculateDto dto)
    {
        Dto = dto;
    }

    public DailyUsageSummaryCalculateDto Dto { get; }
}
