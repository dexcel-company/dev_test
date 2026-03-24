using CelloPark.Application.Common.Filtering.Extensions;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Domain.Common.Enums.CalculationTypes;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Plans.Constants;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Plans.Extensions;

public static class PlanExtensions
{
    public static ErrorOr<Plan> ToModel(this PlanCreateDto dto)
    {
        ErrorOr<Plan> createResult = Plan.Create(
            shadowId: dto.ShadowId,
            name: dto.Name,
            description: dto.Description,
            contractType: ContractType.FromKey(dto.ContractType),
            calculationType: CalculationType.FromKey(dto.CalculationType),
            price: dto.Price,
            vat: dto.HasVat ? PlanSettings.VatDefaultValue : PlanSettings.VatMinValue,
            startDate: dto.StartDate,
            endDate: dto.EndDate);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        return createResult.Value;
    }

    public static ErrorOr<Plan> Update(this Plan model, PlanUpdateDto dto)
    {
        ContractType? contractType = ContractType.FromKey(dto.ContractType);
        CalculationType? calculationType = CalculationType.FromKey(dto.CalculationType);

        ErrorOr<None> shadowIdResult = model.UpdateShadowId(dto.ShadowId);
        ErrorOr<None> nameResult = model.UpdateName(dto.Name);
        ErrorOr<None> descriptionResult = model.UpdateDescription(dto.Description);
        ErrorOr<None> contractTypeIdResult = model.UpdateContractType(contractType);
        ErrorOr<None> calculationTypeIdResult = model.UpdateCalculationType(calculationType);
        ErrorOr<None> priceResult = model.UpdatePrice(dto.Price);
        ErrorOr<None> vatResult = model.UpdateVat(dto.HasVat ? PlanSettings.VatDefaultValue : PlanSettings.VatMinValue);
        ErrorOr<None> startDateResult = model.UpdateStartDate(dto.StartDate);
        ErrorOr<None> endDateResult = model.UpdateEndDate(dto.EndDate);

        List<Error> errors = ErrorProvider.Join(
            shadowIdResult,
            nameResult,
            descriptionResult,
            contractTypeIdResult,
            calculationTypeIdResult,
            priceResult,
            vatResult,
            startDateResult,
            endDateResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return model;
    }

    public static IQueryable<Plan> ApplyFiltering(
        this IQueryable<Plan> source, PlanFilteringCriteria filteringCriteria)
    {
        if (!string.IsNullOrWhiteSpace(filteringCriteria.Status)
            && Enum.TryParse(filteringCriteria.Status, true, out Status status))
        {
            if (Enum.IsDefined(status))
            {
                source = source
                    .IgnoreQueryFilters()
                    .Where(plan => plan.Status == status);
            }
        }

        if (!string.IsNullOrWhiteSpace(filteringCriteria.Search))
        {
            source = source
                .Where(plan => EF.Functions.Like(plan.Name, $"%{filteringCriteria.Search}%"));
        }

        return source;
    }

    public static IOrderedQueryable<Plan> ApplySorting(
        this IQueryable<Plan> source, SortingCriteria sortingCriteria)
    {
        if (string.IsNullOrWhiteSpace(sortingCriteria.Sort))
        {
            return source.OrderBy(plan => plan.Id);
        }

        return sortingCriteria.Sort switch
        {
            _ when string.Equals(nameof(Plan.Name), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.Name, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Plan.Price), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.Price, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Plan.ContractType), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.ContractType, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Plan.Status), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.Status, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Plan.CreateDetails.CreatedAt), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.CreateDetails.CreatedAt, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Plan.CreateDetails.CreatedBy), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(plan => plan.CreateDetails.CreatedBy, sortingCriteria.SortMethod),
            _ =>
                source.OrderBy(plan => plan.Id),
        };
    }
}
