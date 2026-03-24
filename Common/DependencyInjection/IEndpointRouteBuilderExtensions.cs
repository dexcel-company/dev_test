using CelloPark.Api.Features.Benefits.Endpoints;
using CelloPark.Api.Features.CalculationTypes.Endpoints;
using CelloPark.Api.Features.ContractTypes.Endpoints;
using CelloPark.Api.Features.Customers.Endpoints;
using CelloPark.Api.Features.DailyUsages.Endpoints;
using CelloPark.Api.Features.Healths.Endpoints;
using CelloPark.Api.Features.Items.Endpoints;
using CelloPark.Api.Features.Packets.Endpoints;
using CelloPark.Api.Features.Plans.Endpoints;
using CelloPark.Api.Features.Users.Endpoints;

namespace CelloPark.Api.Common.DependencyInjection;

public static class IEndpointRouteBuilderExtensions
{
    public static void UseEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.AddheathEndpoints();
        builder.AddUserEndpoints();
        builder.AddContractTypeEndpoints();
        builder.AddCalculationTypeEndpoints();
        builder.AddItemEndpoints();
        builder.AddPackageEndpoints();
        builder.AddPlanEndpoints();
        builder.AddBenefitEndpoints();
        builder.AddCustomerEndpoints();
        builder.AddDashboardEndpoints();
        builder.AddAmountTypeEndpoints();
        builder.AddCouponTypeEndpoints();
        builder.AddFrequencyTypeEndpoints();
    }
}
