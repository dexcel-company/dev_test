using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCredits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Application.Features.Customers.Queries.GetById.Abstractions;
using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Application.Features.Packets.Dtos;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Domain.Features.Customers.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Queries.GetById;

internal sealed class GetCustomerByIdQueryHandler :
    IGetCustomerByIdQueryHandler
{
    public GetCustomerByIdQueryHandler(IManagementContext manageContext)
    {
        _managementContext = manageContext;
    }

    private readonly IManagementContext _managementContext;

    public async Task<ErrorOr<CustomerGetDto>> HandleAsync(
        GetCustomerByIdQuery request, CancellationToken cancellationToken = default)
    {
        CustomerGetDto? customerGetDto = await _managementContext.Customers
            .Where(customer => customer.Id == request.CustomerId)
            .AsSplitQuery()
            .Select(customer => new CustomerGetDto
            {
                Id = customer.Id,
                Name = customer.Name,
                ContractType = customer.ContractType,
                CustomerPlan = customer.Plan == null ? null : new CustomerPlanGetDto
                {
                    Id = customer.Plan.Id,
                    Plan = new PlanLiteDto
                    {
                        Id = customer.Plan.Plan.Id,
                        Name = customer.Plan.Plan.Name,
                    },
                    CustomerPackages = customer.Plan.PlanPackages.Select(CustomerPackage => new CustomerPackagePageDto
                    {
                        Id = CustomerPackage.Id,
                        Package = new PackageLiteDto
                        {
                            Id = CustomerPackage.Package.Id,
                            Name = CustomerPackage.Package.Name,
                        },
                        Price = CustomerPackage.Price,
                        Vat = CustomerPackage.Vat,
                    }).ToList(),
                    Price = customer.Plan.Price == null
                        ? customer.Plan.Plan.Price
                        : customer.Plan.Price.Value,
                    Vat = customer.Plan.Vat == null
                        ? customer.Plan.Plan.Vat
                        : customer.Plan.Vat.Value,
                },
                CustomerCars = customer.Cars.Select(customerCar => new CustomerCarPageDto
                {
                    Id = customerCar.Id,
                    Number = customerCar.Number,
                }).ToList(),
                CustomerBenefits = customer.Benefits.Select(customerBenefit => new CustomerBenefitPageDto
                {
                    Id = customerBenefit.Id,
                    Benefit = new BenefitLiteDto
                    {
                        Id = customerBenefit.Benefit.Id,
                        Name = customerBenefit.Benefit.Name,
                    },
                    CouponUsage = customer.CouponUsages
                        .Where(x => x.BenefitId == customerBenefit.BenefitId)
                        .Select(x => new CustomerCouponUsagePageDto
                        {
                            Id = x.Id,
                            Coupon = x.Coupon,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                        })
                        .FirstOrDefault(),
                    StartDate = customerBenefit.StartDate,
                    EndDate = customerBenefit.EndDate,
                }).ToList(),
                CustomerCredits = customer.Credits.Select(customerCredit => new CustomerCreditPageDto
                {
                    Id = customerCredit.Id,
                    Item = new ItemLiteDto
                    {
                        Id = customerCredit.Item.Id,
                        Name = customerCredit.Item.Name,
                    },
                    Balance = customerCredit.Balance,
                }).ToList(),
                CreatedAt = customer.CreateDetails.CreatedAt,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (customerGetDto is null)
        {
            return CustomerErrors.NotFound;
        }

        return customerGetDto;
    }
}
