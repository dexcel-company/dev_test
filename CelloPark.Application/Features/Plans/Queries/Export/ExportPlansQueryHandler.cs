using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Plans.Queries.Export.Abstractions;
using CelloPark.Application.Features.Plans.Services.Abstractions;
using CelloPark.Domain.Features.Plans;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Plans.Queries.Export;

internal sealed class ExportPlansQueryHandler :
    IExportPlansQueryHandler
{
    public ExportPlansQueryHandler(
        IManagementContext managementContext,
        IPlanExportService planExportService)
    {
        _managementContext = managementContext;
        _planExportService = planExportService;
    }

    private readonly IManagementContext _managementContext;
    private readonly IPlanExportService _planExportService;

    public async Task<FileStream> HandleAsync(
        ExportPlansQuery request, CancellationToken cancellationToken = default)
    {
        List<Plan> plans = await GetPlansAsync(cancellationToken);

        return _planExportService.Export(plans);
    }

    private async Task<List<Plan>> GetPlansAsync(CancellationToken cancellationToken)
    {
        return await _managementContext.Plans
            .AsNoTracking()
            .Include(plan => plan.PlanPackages)
                .ThenInclude(planPackage => planPackage.Package)
            .ToListAsync(cancellationToken);
    }
}
