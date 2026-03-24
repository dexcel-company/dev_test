using CelloPark.Application.Features.Benefits.Dtos;

namespace CelloPark.Application.Features.Benefits.Commands.Update;

public sealed class UpdateBenefitCommand
{
    public UpdateBenefitCommand(Guid benefitId, BenefitUpdateDto dto)
    {
        BenefitId = benefitId;
        Dto = dto;
    }

    public Guid BenefitId { get; }
    public BenefitUpdateDto Dto { get; }
}
