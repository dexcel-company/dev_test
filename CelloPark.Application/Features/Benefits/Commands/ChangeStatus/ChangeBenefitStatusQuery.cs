namespace CelloPark.Application.Features.Benefits.Commands.ChangeStatus;

public sealed class ChangeBenefitStatusQuery
{
    public ChangeBenefitStatusQuery(Guid benefitId)
    {
        BenefitId = benefitId;
    }

    public Guid BenefitId { get; }
}
