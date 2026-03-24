using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Packets.Extensions;
using CelloPark.Application.Features.Packets.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Packets.Queries.GetAll;

internal sealed class GetAllPackagesQueryHandler :
    IGetAllPackagesQueryHandler
{
    public GetAllPackagesQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<Page<PackagePageDto>> HandleAsync(
        GetAllPackagesQuery request, CancellationToken cancellationToken = default)
    {
        Page<PackagePageDto> packagePage = await _managementContext.Packages
            .ApplyFiltering(request.FilteringCriteria)
            .ApplySorting(request.SortingCriteria)
            .Select(package => new PackagePageDto
            {
                Id = package.Id,
                ShadowId = package.ShadowId,
                Name = package.Name,
                Status = package.Status.ToString(),
                RelatedPlans = package.PlanPackages.Select(planPackage => new PlanLiteDto
                {
                    Id = planPackage.Plan.Id,
                    Name = planPackage.Plan.Name,
                }).ToList(),
                CreatedAt = package.CreateDetails.CreatedAt,
                CreatedBy = package.CreateDetails.User == null ? null : new UserAuditDto
                {
                    Id = package.CreateDetails.User.Id,
                    FirstName = package.CreateDetails.User.FirstName,
                    LastName = package.CreateDetails.User.LastName,
                }
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return packagePage;
    }
}
