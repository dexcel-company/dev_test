using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Exports.Abstractions;
using CelloPark.Application.Features.Benefits.Queries.Export.Abstractions;
using CelloPark.Domain.Features.Benefits;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Benefits.Queries.Export;

internal sealed class ExportBenefitsQueryHandler :
    IExportBenefitsQueryHandler
{
    public ExportBenefitsQueryHandler(
        IManagementContext managementContext,
        IBenefitExportService benefitExportService)
    {
        _managementContext = managementContext;
        _benefitExportService = benefitExportService;
    }

    private readonly IManagementContext _managementContext;
    private readonly IBenefitExportService _benefitExportService;

    public async Task<FileStream> HandleAsync(
        ExportBenefitsQuery request, CancellationToken cancellationToken = default)
    {
        List<Benefit> benefits = await GetBenefitsAsync(cancellationToken);

        return _benefitExportService.Export(benefits);
    }

    private async Task<List<Benefit>> GetBenefitsAsync(CancellationToken cancellationToken)
    {
        return await _managementContext.Benefits
            .Include(benefit => benefit.PaymentCategories)
            .Include(benefit => benefit.Coupons)
            .ToListAsync(cancellationToken);
    }
}
