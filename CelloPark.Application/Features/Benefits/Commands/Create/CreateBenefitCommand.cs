using CelloPark.Application.Features.Benefits.Dtos;

namespace CelloPark.Application.Features.Benefits.Commands.Create;

public sealed class CreateBenefitCommand
{
    public CreateBenefitCommand(BenefitCreateDto dto)
    {
        Dto = dto;
    }

    public BenefitCreateDto Dto { get; }
}
