using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Benefits.Extensions;
using CelloPark.Application.Features.Benefits.Queries.GetAll.Abstractions;
using CelloPark.Application.Features.Users.Dtos;

namespace CelloPark.Application.Features.Benefits.Queries.GetAll;

internal sealed class GetAllBenefitsQueryHandler :
    IGetAllBenefitsQueryHandler
{
    public GetAllBenefitsQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<Page<BenefitPageDto>> HandleAsync(
        GetAllBenefitsQuery request, CancellationToken cancellationToken = default)
    {
        Page<BenefitPageDto> benefitPage = await _managementContext.Benefits
            .ApplyFiltering(request.FilteringCriteria)
            .ApplySorting(request.SortingCriteria)
            .Select(benefit => new BenefitPageDto
            {
                Id = benefit.Id,
                Name = benefit.Name,
                Applied = benefit.PaymentCategories.All(x => x.ItemId != null) ? "Items" :
                    benefit.PaymentCategories.All(x => x.PackageId != null) ? "Package" :
                    benefit.PaymentCategories.All(x => x.PlanId != null) ? "Plan" : "Unknown",
                StartPrometionDate = benefit.StartPromotionDate,
                EndPromotionDate = benefit.EndPromotionDate,
                Coupons = benefit.Coupons.Count(),
                PaymentCategories = benefit.PaymentCategories.Select(paymentCategory => new BenefitPaymentCategoryPageDto
                {
                    Plan = paymentCategory.PlanId,
                    Package = paymentCategory.PackageId,
                    Item = paymentCategory.ItemId,
                    Amount = paymentCategory.Amount,
                    AmountType = paymentCategory.AmountType,
                    Frequency = paymentCategory.Frequency,
                    FrequencyType = paymentCategory.FrequencyType,
                    AmountLimit = paymentCategory.AmountLimit
                }).ToList(),
                Status = benefit.Status.ToString(),
                CreatedAt = benefit.CreateDetails.CreatedAt,
                CreatedBy = benefit.CreateDetails.User == null ? null : new UserAuditDto
                {
                    Id = benefit.CreateDetails.User.Id,
                    FirstName = benefit.CreateDetails.User.FirstName,
                    LastName = benefit.CreateDetails.User.LastName,
                },
            })
            .ApplyPaginationAsync(request.PaginationCriteria, cancellationToken);

        return benefitPage;
    }
}
