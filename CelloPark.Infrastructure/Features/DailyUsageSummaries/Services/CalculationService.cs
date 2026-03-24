using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitCoupons.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerCars.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerDailyCharges.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPlans.Dtos;
using CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;
using CelloPark.Application.Features.Plans.Dtos;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Abstractions;
using CelloPark.Infrastructure.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Features.DailyUsageSummaries.Services;

internal sealed class CalculationService :
    ICalculationService
{
    private const int DayStep = 1;
    private const int DefaultHour = 23;
    private const int DefaultMinute = 59;
    private const int DefaultSecond = 59;

    public CalculationService(
        ICalculationWorker calculationWorker)
    {
        _calculationWorker = calculationWorker;
    }

    private readonly ICalculationWorker _calculationWorker;

    public async Task ExecuteAsync(
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, DefaultHour, DefaultMinute, DefaultSecond);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, DefaultHour, DefaultMinute, DefaultSecond);
        TimeSpan dateDifference = (endDate - startDate);

        using DailyUsageContext dailyUsageContext = new();

        for (int i = 0; i <= dateDifference.Days; i++)
        {
            DateOnly snapshotDate = new(startDate.Year, startDate.Month, startDate.Day);

            await DeleteUsagesAsync(dailyUsageContext, snapshotDate, cancellationToken);

            await dailyUsageContext.CustomerBenefits
                .Where(customerBenefit => customerBenefit.EndDate < startDate)
                .ExecuteUpdateAsync(customerBenefit => customerBenefit.SetProperty(property => property.Status, Status.Deleted), cancellationToken);

            List<CustomerCalculationDto> customers = await GetCustomersAsync(dailyUsageContext, snapshotDate, cancellationToken);
            List<BenefitCalculationDto> benefits = await GetBenefitsAsync(dailyUsageContext, snapshotDate, cancellationToken);

            await _calculationWorker.ExecuteAsync(customers, benefits, startDate, cancellationToken);

            startDate = startDate.AddDays(DayStep);
        }
    }

    private static async Task DeleteUsagesAsync(
        DailyUsageContext dailyUsageContext, DateOnly date, CancellationToken cancellationToken = default)
    {
        await dailyUsageContext.DailyItemUsageSummaries
            .Where(summary => summary.Date == date)
            .ExecuteDeleteAsync(cancellationToken);

        await dailyUsageContext.DailyPlanUsageSummaries
            .Where(summary => summary.Date == date)
            .ExecuteDeleteAsync(cancellationToken);

        await dailyUsageContext.DailyPackageUsageSummaries
            .Where(summary => summary.Date == date)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static async Task<List<CustomerCalculationDto>> GetCustomersAsync(
        DailyUsageContext dailyUsageContext, DateOnly snapshotDate, CancellationToken cancellationToken = default)
    {
        List<CustomerCalculationDto> customers = await dailyUsageContext.CustomerSnapshots
            .Where(snapshot => snapshot.SnapshotDate == snapshotDate)
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
                    StartDate = null,
                    EndDate = null,
                    Plan = new PlanCalculationDto
                    {
                        Id = customer.Plan.Plan.Id,
                        ShadowId = customer.Plan.Plan.ShadowId,
                        ContractType = customer.Plan.Plan.ContractType,
                        CalculationType = customer.Plan.Plan.CalculationType,
                        Price = customer.Plan.Plan.Price,
                        Vat = customer.Plan.Plan.Vat,
                        StartDate = null,
                        EndDate = null,
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
                            StartDate = null,
                            EndDate = null,
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
                        StartDate = null,
                        EndDate = null,
                    })
                    .ToList(),
                Cars = customer.Cars
                    .Select(customerCar => new CustomerCarCalculationDto
                    {
                        Id = customerCar.Id,
                        CustomerId = customerCar.CustomerId,
                        Number = customerCar.Number,
                        StartDate = null,
                        EndDate = null,
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
            })
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return customers;
    }

    private static async Task<List<BenefitCalculationDto>> GetBenefitsAsync(
        DailyUsageContext dailyUsageContext, DateOnly snapshotDate, CancellationToken cancellationToken = default)
    {
        List<BenefitCalculationDto> benefits = await dailyUsageContext.BenefitSnapshots
            .Where(snapshot => snapshot.SnapshotDate == snapshotDate)
            .Select(benefit => new BenefitCalculationDto
            {
                Id = benefit.Id,
                Name = benefit.Name,
                StartActiveDate = benefit.StartActiveDate,
                EndActiveDate = benefit.EndActiveDate,
                StartPromotionDate = benefit.StartPromotionDate,
                EndPromotionDate = benefit.EndPromotionDate,
                Duration = benefit.Duration,
                CouponsDuration = benefit.CouponsDuration,
                PaymentCategories = benefit.PaymentCategories
                    .Select(paymentCategory => new BenefitPaymentCategoryCalculationDto
                    {
                        Id = paymentCategory.Id,
                        ItemId = paymentCategory.ItemId,
                        PlanId = paymentCategory.PlanId,
                        PackageId = paymentCategory.PackageId,
                        BenefitId = paymentCategory.BenefitId,
                        Amount = paymentCategory.Amount,
                        AmountLimit = paymentCategory.AmountLimit,
                        AmountType = paymentCategory.AmountType,
                        Frequency = paymentCategory.Frequency,
                        FrequencyType = paymentCategory.FrequencyType,
                    })
                    .ToList(),
                Coupons = benefit.Coupons
                    .Select(coupon => new BenefitCouponCalculationDto
                    {
                        Id = coupon.Id,
                        BenefitId = coupon.BenefitId,
                        Coupon = coupon.Coupon,
                        CouponType = coupon.CouponType,
                        Duration = coupon.Duration,
                        Status = coupon.Status,
                    })
                    .ToList(),
            })
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return benefits;
    }
}
