using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Plans.Extensions;
using CelloPark.Application.Features.Plans.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Plans.Queries.GetAll;

internal sealed class GetAllPlansQueryHandler :
    IGetAllPlansQueryHandler
{
    public GetAllPlansQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<Page<PlanPageDto>> HandleAsync(
        GetAllPlansQuery request, CancellationToken cancellationToken = default)
    {
        Page<PlanPageDto> planPage = await _managementContext.Plans
            .ApplyFiltering(request.FilteringCriteria)
            .ApplySorting(request.SortingCriteria)
            .Select(plan => new PlanPageDto
            {
                Id = plan.Id,
                ShadowId = plan.ShadowId,
                Name = plan.Name,
                Status = plan.Status.ToString(),
                Price = plan.Price,
                ContractType = plan.ContractType,
                CalculationType = plan.CalculationType,
                CreatedAt = plan.CreateDetails.CreatedAt,
                CreatedBy = plan.CreateDetails.User == null ? null : new UserAuditDto
                {
                    Id = plan.CreateDetails.User.Id,
                    FirstName = plan.CreateDetails.User.FirstName,
                    LastName = plan.CreateDetails.User.LastName,
                },
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return planPage;
    }
}
