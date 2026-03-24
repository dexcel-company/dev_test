using CelloPark.Domain.Features.Benefits;

namespace CelloPark.Application.Features.Benefits.Exports.Abstractions;

public interface IBenefitExportService
{
    FileStream Export(List<Benefit> benefits);
}
