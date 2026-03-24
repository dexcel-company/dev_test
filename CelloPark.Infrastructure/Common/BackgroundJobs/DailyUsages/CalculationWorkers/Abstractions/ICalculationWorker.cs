using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Customers.Dtos;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Abstractions;

internal interface ICalculationWorker
{
    Task ExecuteAsync(
        List<CustomerCalculationDto> customers,
        List<BenefitCalculationDto> benefits,
        DateTime datetime,
        CancellationToken cancellationToken = default);
}
