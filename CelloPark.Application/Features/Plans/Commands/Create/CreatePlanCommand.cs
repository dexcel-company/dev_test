using CelloPark.Application.Features.Plans.Dtos;

namespace CelloPark.Application.Features.Plans.Commands.Create;

public sealed class CreatePlanCommand
{
    public CreatePlanCommand(PlanCreateDto dto)
    {
        Dto = dto;
    }

    public PlanCreateDto Dto { get; }
}
