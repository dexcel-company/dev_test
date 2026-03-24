using CelloPark.Domain.Features.Plans;

namespace CelloPark.Application.Features.Plans.Services.Abstractions;

public interface IPlanExportService
{
    FileStream Export(List<Plan> plans);
}
