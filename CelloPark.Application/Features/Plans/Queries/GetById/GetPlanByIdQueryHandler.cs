using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Plans.Queries.GetById.Abstractions;
using CelloPark.Application.Features.Users.Dtos;
using CelloPark.Domain.Features.Plans.Constants;
using CelloPark.Domain.Features.Plans.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Plans.Queries.GetById;

internal sealed class GetPlanByIdQueryHandler :
    IGetPlanByIdQueryHandler
{
    public GetPlanByIdQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<PlanGetDto>> HandleAsync(
        GetPlanByIdQuery request, CancellationToken cancellationToken = default)
    {
        PlanGetDto? planGetDto = await _managementContext.Plans
            .Where(plan => plan.Id == request.PlanId)
            .Select(plan => new PlanGetDto
            {
                Id = plan.Id,
                ShadowId = plan.ShadowId,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                ContractType = plan.ContractType,
                CalculationType = plan.CalculationType,
                HasVat = plan.Vat == PlanSettings.VatDefaultValue,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                Packages = plan.PlanPackages.Select(planPackage => new PackagePageDto
                {
                    Id = planPackage.Package.Id,
                    ShadowId = planPackage.Package.ShadowId,
                    Name = planPackage.Package.Name,
                    Status = planPackage.Package.Status.ToString(),
                    RelatedPlans = null!,
                    CreatedAt = planPackage.Package.CreateDetails.CreatedAt,
                    CreatedBy = planPackage.CreateDetails.User == null ? null : new UserAuditDto
                    {
                        Id = planPackage.CreateDetails.User.Id,
                        FirstName = planPackage.CreateDetails.User.FirstName,
                        LastName = planPackage.CreateDetails.User.LastName,
                    },
                }).ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (planGetDto is null)
        {
            return PlanErrors.NotFound;
        }

        return planGetDto;
    }
}
