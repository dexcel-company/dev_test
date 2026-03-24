namespace CelloPark.Application.Features.Benefits.Commands.Delete;

public sealed class DeleteBenefitCommand
{
    public DeleteBenefitCommand(Guid benefitId)
    {
        BenefitId = benefitId;
    }

    public Guid BenefitId { get; }
}
