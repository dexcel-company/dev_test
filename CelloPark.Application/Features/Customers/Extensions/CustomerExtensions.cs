using CelloPark.Application.Common.Filtering.Extensions;
using CelloPark.Application.Common.Sorting;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerDailyCharges.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Domain.Features.Customers;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Application.Features.Customers.Extensions;

public static class CustomerExtensions
{
    public static IQueryable<Customer> ApplyFiltering(
        this IQueryable<Customer> source, CustomerFilteringQuery filteringCriteria)
    {
        if (!string.IsNullOrWhiteSpace(filteringCriteria.Coupon))
        {
            source = source
                .Where(customer => customer.CouponUsages.Any(coupon => coupon.Coupon == filteringCriteria.Coupon));
        }

        if (!string.IsNullOrEmpty(filteringCriteria.Search))
        {
            source = source
                .Where(customer => EF.Functions.Like(customer.Name, $"%{filteringCriteria.Search}%"));
        }

        return source;
    }

    public static IQueryable<Customer> ApplySorting(
        this IQueryable<Customer> source, SortingCriteria sortingCriteria)
    {
        if (string.IsNullOrEmpty(sortingCriteria.Sort))
        {
            return source.OrderBy(customer => customer.Id);
        }

        return sortingCriteria.Sort switch
        {
            _ when string.Equals("Name", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.Name, sortingCriteria.SortMethod),
            _ when string.Equals("PlanName", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.Plan.Plan.Name, sortingCriteria.SortMethod),
            _ when string.Equals("CustomerPlanPrice", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.Plan.Price, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Customer.ContractType), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.ContractType, sortingCriteria.SortMethod),
            _ when string.Equals("PackageCount", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.Plan.PlanPackages.Count, sortingCriteria.SortMethod),
            _ when string.Equals("CarCount", sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.Cars.Count, sortingCriteria.SortMethod),
            _ when string.Equals(nameof(Customer.CreateDetails.CreatedAt), sortingCriteria.Sort, StringComparison.InvariantCultureIgnoreCase) =>
                source.OrderBy(customer => customer.CreateDetails.CreatedAt, sortingCriteria.SortMethod),
            _ =>
                source.OrderBy(customer => customer.Id),
        };
    }

    public static IQueryable<CustomerCalculationDto> AsCalculationDto(
        this IQueryable<Customer> source)
    {
        return source
            .Select(customer => new CustomerCalculationDto
            {
                Id = customer.Id,
                ContractType = customer.ContractType,
                CustomerPlanId = customer.CustomerPlanId,
                Plan = new CustomerPlanCalculationDto
                {
                    Id = customer.Plan.Id,
                    PlanId = customer.Plan.PlanId,
                    Price = customer.Plan.Price,
                    Vat = customer.Plan.Vat,
                    StartDate = customer.Plan.StartDate,
                    EndDate = customer.Plan.EndDate,
                    Plan = new PlanCalculationDto
                    {
                        Id = customer.Plan.Plan.Id,
                        ShadowId = customer.Plan.Plan.ShadowId,
                        ContractType = customer.Plan.Plan.ContractType,
                        CalculationType = customer.Plan.Plan.CalculationType,
                        Price = customer.Plan.Plan.Price,
                        Vat = customer.Plan.Plan.Vat,
                        StartDate = customer.Plan.Plan.StartDate,
                        EndDate = customer.Plan.Plan.EndDate,
                    },
                    PlanPackages = customer.Plan.PlanPackages
                        .Select(CustomerPackage => new CustomerPackageCalculationDto
                        {
                            Id = CustomerPackage.Id,
                            PackageId = CustomerPackage.PackageId,
                            CustomerCarId = CustomerPackage.CustomerCarId,
                            CustomerPlanId = CustomerPackage.CustomerPlanId,
                            Price = CustomerPackage.Price,
                            Vat = CustomerPackage.Vat,
                            StartDate = CustomerPackage.StartDate,
                            EndDate = CustomerPackage.EndDate,
                        })
                        .ToList(),
                },
                Benefits = customer.Benefits
                    .Select(customerBenefit => new CustomerBenefitCalculationDto
                    {
                        Id = customerBenefit.Id,
                        CustomerId = customerBenefit.CustomerId,
                        BenefitId = customerBenefit.BenefitId,
                        Debit = customerBenefit.Debit,
                        LimitAmountLeft = customerBenefit.LimitAmountLeft,
                        FrequencyCountLeft = customerBenefit.FrequencyCountLeft,
                        StartDate = customerBenefit.StartDate,
                        EndDate = customerBenefit.EndDate,
                    })
                    .ToList(),
                Cars = customer.Cars
                    .Select(customerCar => new CustomerCarCalculationDto
                    {
                        Id = customerCar.Id,
                        CustomerId = customerCar.CustomerId,
                        Number = customerCar.Number,
                        StartDate = customerCar.StartDate,
                        EndDate = customerCar.EndDate,
                    })
                    .ToList(),
                DailyCharges = customer.DailyCharges
                    .Select(customerDailyCharge => new CustomerDailyChargeCalculationDto
                    {
                        Id = customerDailyCharge.Id,
                        CustomerId = customerDailyCharge.CustomerId,
                        CustomerCarId = customerDailyCharge.CustomerCarId,
                        ItemId = customerDailyCharge.ItemId,
                        Price = customerDailyCharge.Price,
                        Count = customerDailyCharge.Count,
                    })
                    .ToList(),
            });
    }
}
