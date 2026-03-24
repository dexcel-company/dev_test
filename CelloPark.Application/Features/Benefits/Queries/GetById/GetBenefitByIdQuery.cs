namespace CelloPark.Application.Features.Benefits.Queries.GetById;

public sealed class GetBenefitByIdQuery
{
    public GetBenefitByIdQuery(Guid benefitId)
    {
        BenefitId = benefitId;
    }

    public Guid BenefitId { get; }
}
