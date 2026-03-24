using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans.Constants;
using ErrorOr;

namespace CelloPark.Application.Features.Customers.Entities.CustomerPlans.Extensions;

public static class CustomerPlanExtensions
{
    public static ErrorOr<CustomerPlan> Update(this CustomerPlan model, CustomerPlanUpdateDto dto)
    {
        ErrorOr<None> priceResult = model.UpdatePrice(dto.Price);
        ErrorOr<None> vatResult = model.UpdateVat(dto.HasVat ? CustomerPlanSettings.VatDefaultValue : CustomerPlanSettings.VatMinValue);

        List<Error> errors = ErrorProvider.Join(
            priceResult, vatResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return model;
    }
}
