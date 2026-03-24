using CelloPark.Application.Features.Benefits.Dtos;
using CelloPark.Application.Features.Benefits.Entities.BenefitPaymentCategories.Dto;
using CelloPark.Application.Features.Customers.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerBenefits.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerDailyCharges.Dtos;
using CelloPark.Application.Features.Customers.Entities.CustomerPackages.Dtos;
using CelloPark.Domain.Features.Benefits.Enums;
using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.CalculationExceptions.Enums;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.DailyItemUsageCalculations;
using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Domain.Features.DailyPackageUsageCalculations;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Domain.Features.DailyPlanCalculations;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Abstractions;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Converters;
using CelloPark.Infrastructure.Common.Environments.Constants;
using CelloPark.Infrastructure.Common.Providers;
using ErrorOr;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers;

internal sealed class CalculationWorker :
    ICalculationWorker
{
    private const int MaxPercent = 100;
    private const int AverageQueryLength = 140;

    public CalculationWorker(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    public async Task ExecuteAsync(
        List<CustomerCalculationDto> customers,
        List<BenefitCalculationDto> benefits,
        DateTime datetime,
        CancellationToken cancellationToken = default)
    {
        if (DailyUsageJobMonitor.IsRunning)
        {
            return;
        }

        //DailyUsageJobMonitor.Lock();

        //int threadCount = int.Parse(Environment.GetEnvironmentVariable(BackgroundJobKeys.ThreadCount)!);
        //int chunkSize = customers.Count / threadCount;
        //int remnant = customers.Count % threadCount;
        //int[] chunks = new int[threadCount];
        //Task[] tasks = new Task[threadCount];

        //for (int i = 1; i < threadCount + 1; i++)
        //{
        //    chunks[i - 1] = chunkSize;
        //}

        //chunks[threadCount - 1] += remnant;

        //for (int i = 0; i < threadCount; i++)
        //{
        //    IEnumerable<CustomerCalculationDto> customerChunk = customers
        //        .Skip(chunkSize * i)
        //        .Take(chunks[i]);

        //    tasks[i] = Task.Run(() =>
        //    {
        //        Calculate(chunkSize, customerChunk, benefits, datetime);
        //    },
        //    cancellationToken);
        //}

        //await Task.WhenAll(tasks);
        //DailyUsageJobMonitor.Unlock();
    }

    //private void Calculate(
    //    int customerCount,
    //    IEnumerable<CustomerCalculationDto> customers,
    //    List<BenefitCalculationDto> benefits,
    //    DateTimeOffset dateTime)
    //{
    //    Dictionary<Guid, DailyItemUsageSummary> itemUsageSummaries = new(customerCount);
    //    Dictionary<Guid, DailyPlanUsageSummary> planUsageSummaries = new(customerCount);
    //    Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries = new(customerCount);

    //    List<DailyItemUsageCalculation> itemUsageCalculations = new(customerCount);
    //    List<DailyPlanUsageCalculation> planUsageCalculations = new(customerCount);
    //    List<DailyPackageUsageCalculation> packageUsageCalculations = new(customerCount);

    //    List<CustomerBenefit> createdCustomerBenefits = new(customerCount / 2 + 10);
    //    List<CustomerBenefitLimitCalculationDto> updatedCustomerBenefits = new(customerCount / 2 + 10);
    //    List<CalculationException> exceptions = new(customerCount / 2 + 10);

    //    foreach (CustomerCalculationDto customer in customers)
    //    {
    //        CalculateDailyUsages(
    //            customer,
    //            benefits,
    //            itemUsageSummaries,
    //            planUsageSummaries,
    //            packageUsageSummaries,
    //            itemUsageCalculations,
    //            planUsageCalculations,
    //            packageUsageCalculations,
    //            createdCustomerBenefits,
    //            updatedCustomerBenefits,
    //            exceptions,
    //            dateTime);
    //    }

    //    SaveChanges(
    //        itemUsageSummaries,
    //        planUsageSummaries,
    //        packageUsageSummaries,
    //        itemUsageCalculations,
    //        planUsageCalculations,
    //        packageUsageCalculations,
    //        createdCustomerBenefits,
    //        updatedCustomerBenefits,
    //        exceptions);
    //}

    //private static void SaveChanges(
    //    Dictionary<Guid, DailyItemUsageSummary> itemUsageSummaries,
    //    Dictionary<Guid, DailyPlanUsageSummary> planUsageSummaries,
    //    Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries,
    //    List<DailyItemUsageCalculation> itemUsageCalculations,
    //    List<DailyPlanUsageCalculation> planUsageCalculations,
    //    List<DailyPackageUsageCalculation> packageUsageCalculations,
    //    List<CustomerBenefit> createdCustomerBenefits,
    //    List<CustomerBenefitLimitCalculationDto> updatedCustomerBenefits,
    //    List<CalculationException> exceptions)
    //{
    //    SqlConnection connection = new(DatabaseProvider.ConnectionString);
    //    connection.Open();

    //    DataTable dataTable = new();
    //    SqlBulkCopyOptions bulkOptions = SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.CheckConstraints;
    //    SqlBulkCopy sqlBulk = new(DatabaseProvider.ConnectionString, bulkOptions);

    //    if (createdCustomerBenefits.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertCustomerBenefits(createdCustomerBenefits);
    //        sqlBulk = sqlBulk.ConvertCustomerBenefits();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (itemUsageSummaries.Values.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertItemUsageSummaries(itemUsageSummaries);
    //        sqlBulk = sqlBulk.ConvertItemUsageSummaries();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (planUsageSummaries.Values.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertPlanUsageSummaries(planUsageSummaries);
    //        sqlBulk = sqlBulk.ConvertPlanUsageSummaries();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (packageUsageSummaries.Values.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertPackageUsageSummaries(packageUsageSummaries);
    //        sqlBulk = sqlBulk.ConvertPackageUsageSummaries();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (itemUsageCalculations.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertItemUsageCalculations(itemUsageCalculations);
    //        sqlBulk = sqlBulk.ConvertItemUsageCalculations();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (planUsageCalculations.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertPlanUsageCalculations(planUsageCalculations);
    //        sqlBulk = sqlBulk.ConvertPlanUsageCalculations();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (packageUsageCalculations.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertPackageUsageCalculations(packageUsageCalculations);
    //        sqlBulk = sqlBulk.ConvertPackageUsageCalculations();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (exceptions.Count > 0)
    //    {
    //        dataTable = dataTable.ConvertCalculationExceptions(exceptions);
    //        sqlBulk = sqlBulk.ConvertCalculationExceptions();
    //        sqlBulk.WriteToServer(dataTable);
    //    }

    //    if (updatedCustomerBenefits.Count > 0)
    //    {
    //        StringBuilder stringBuilder = new(updatedCustomerBenefits.Count * AverageQueryLength);

    //        foreach (CustomerBenefitLimitCalculationDto customerBenefit in updatedCustomerBenefits)
    //        {
    //            string query = $"""
    //                UPDATE [CustomerBenefit]
    //                SET
    //                    [FrequencyCountLeft] = {customerBenefit.FrequencyCountLeft}
    //                    ,[LimitAmountLeft] = {customerBenefit.LimitAmountLeft}
    //                WHERE [Id] = {customerBenefit.Id};
    //            """;

    //            stringBuilder.Append(query);
    //        }

    //        string sql = stringBuilder.ToString();
    //        SqlCommand command = new(sql, connection);

    //        command.ExecuteNonQuery();
    //    }

    //    connection.Close();
    //    sqlBulk.Close();
    //}

    //private void CalculateDailyUsages(
    //    CustomerCalculationDto customer,
    //    List<BenefitCalculationDto> benefits,
    //    Dictionary<Guid, DailyItemUsageSummary> itemUsageSummaries,
    //    Dictionary<Guid, DailyPlanUsageSummary> planUsageSummaries,
    //    Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries,
    //    List<DailyItemUsageCalculation> dailyItemUsageCalculations,
    //    List<DailyPlanUsageCalculation> dailyPlanUsageCalculations,
    //    List<DailyPackageUsageCalculation> dailyPackageUsageCalculations,
    //    List<CustomerBenefit> createdCustomerBenefits,
    //    List<CustomerBenefitLimitCalculationDto> updatedCustomerBenefits,
    //    List<CalculationException> exceptions,
    //    DateTimeOffset dateTime)
    //{
    //    IEnumerable<BenefitCalculationDto> expectedCustomerBenefits = benefits
    //        .Where(benefit => benefit.PaymentCategories.All(paymentCategory => paymentCategory.PlanId == customer.Plan.PlanId)
    //            || customer.Plan.PlanPackages.Any(CustomerPackage => benefit.PaymentCategories.All(paymentCategory => paymentCategory.PackageId == CustomerPackage.PackageId))
    //            || (benefit.PaymentCategories.All(paymentCategory => paymentCategory.PlanId is null)
    //            && benefit.PaymentCategories.All(paymentCategory => paymentCategory.PackageId is null)));

    //    BenefitCalculationDto planBenefit = null!;
    //    List<BenefitCalculationDto> packageBenefits = new(dailyPackageUsageCalculations.Capacity / 2 + 10);
    //    List<BenefitCalculationDto> itemBenefits = new(dailyItemUsageCalculations.Capacity / 2 + 10);

    //    foreach (BenefitCalculationDto benefit in expectedCustomerBenefits)
    //    {
    //        if (customer.Benefits.Any(customerBenefit => customerBenefit.BenefitId == benefit.Id))
    //        {
    //            continue;
    //        }

    //        if (IsLinkableBenefit(benefit, dateTime.UtcDateTime))
    //        {
    //            bool isNew = false;

    //            if (benefit.PaymentCategories.All(paymentCategory => paymentCategory.PlanId == customer.Plan.PlanId)
    //                && benefit.PaymentCategories.All(paymentCategory => paymentCategory.PackageId is null)
    //                && benefit.PaymentCategories.All(paymentCategory => paymentCategory.ItemId is null))
    //            {
    //                planBenefit = benefit;
    //                isNew = true;
    //            }
    //            else if (customer.Plan.PlanPackages.Any(CustomerPackage => benefit.PaymentCategories.All(paymentCategory => paymentCategory.PackageId == CustomerPackage.Id))
    //                && benefit.PaymentCategories.All(paymentCategory => paymentCategory.PlanId is null)
    //                && benefit.PaymentCategories.All(paymentCategory => paymentCategory.ItemId is null))
    //            {
    //                packageBenefits.Add(benefit);
    //                isNew = true;
    //            }
    //            else
    //            {
    //                itemBenefits.Add(benefit);
    //                isNew = true;
    //            }

    //            if (isNew)
    //            {
    //                ErrorOr<CustomerBenefit> customerBenefitResult = CustomerBenefit.Create(
    //                    customerId: customer.Id,
    //                    benefitId: benefit.Id,
    //                    debit: benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount),
    //                    frequencyCountLeft: benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Frequency),
    //                    limitAmountLeft: benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.AmountLimit),
    //                    startDate: benefit.StartActiveDate,
    //                    endDate: benefit.EndActiveDate);

    //                if (customerBenefitResult.IsError)
    //                {
    //                    IReadOnlyCollection<CalculationException> remoteExeptions = CalculationException.Create(
    //                        customerBenefitResult.Errors,
    //                        type: CalculationExceptionType.Internal,
    //                        dateTime: _timeProvider.GetUtcNow());

    //                    exceptions.AddRange(remoteExeptions);
    //                }

    //                createdCustomerBenefits.Add(customerBenefitResult.Value);
    //            }
    //        }
    //    }

    //    IEnumerable<DailyItemUsageCalculation> itemUsages = CreateItemDailyUsageCalculation(
    //        customer,
    //        itemBenefits,
    //        itemUsageSummaries,
    //        updatedCustomerBenefits,
    //        exceptions,
    //        dateTime);

    //    IEnumerable<DailyPackageUsageCalculation> packageUsages = CreatePackageDailyUsageCalculation(
    //        customer,
    //        packageBenefits,
    //        packageUsageSummaries,
    //        exceptions,
    //        dateTime);

    //    DailyPlanUsageCalculation planUsage = CreatePlanDailyUsageCalculation(
    //        customer,
    //        planBenefit,
    //        planUsageSummaries,
    //        exceptions,
    //        dateTime);

    //    dailyItemUsageCalculations.AddRange(itemUsages);
    //    dailyPackageUsageCalculations.AddRange(packageUsages);
    //    dailyPlanUsageCalculations.Add(planUsage);
    //}

    //private static bool IsLinkableBenefit(
    //    BenefitCalculationDto benefit,
    //    DateTime dateTime)
    //{
    //    if (benefit.StartPromotionDate is null
    //        && benefit.EndPromotionDate is null)
    //    {
    //        return true;
    //    }

    //    if (benefit.StartPromotionDate < dateTime
    //        && benefit.EndPromotionDate is null)
    //    {
    //        return true;
    //    }

    //    if (benefit.StartPromotionDate is null
    //        && benefit.EndPromotionDate > dateTime)
    //    {
    //        return true;
    //    }

    //    if (benefit.StartPromotionDate < dateTime
    //        && benefit.EndPromotionDate > dateTime)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private DailyPlanUsageCalculation CreatePlanDailyUsageCalculation(
    //    CustomerCalculationDto customer,
    //    BenefitCalculationDto planBenefit,
    //    Dictionary<Guid, DailyPlanUsageSummary> dailyPlanUsageSummaries,
    //    List<CalculationException> exceptions,
    //    DateTimeOffset utcNow)
    //{
    //    (decimal gross, decimal cost, decimal benefit) = CalculatePlanDailyUsage(customer, planBenefit, utcNow);
    //    CalculatePlanSummary(dailyPlanUsageSummaries, customer.Plan.PlanId, gross, cost, utcNow);

    //    ErrorOr<DailyPlanUsageCalculation> dailyPlanUsageCalculationResult = DailyPlanUsageCalculation.Create(
    //        customerId: customer.Id,
    //        customerPlanId: customer.CustomerPlanId,
    //        benefitId: planBenefit?.Id ?? null,
    //        cost: cost,
    //        carCount: customer.Cars.Count);

    //    if (dailyPlanUsageCalculationResult.IsError)
    //    {
    //        IReadOnlyCollection<CalculationException> remoteExeptions = CalculationException.Create(
    //            dailyPlanUsageCalculationResult.Errors,
    //            type: CalculationExceptionType.Internal,
    //            dateTime: _timeProvider.GetUtcNow());

    //        exceptions.AddRange(remoteExeptions);
    //    }

    //    return dailyPlanUsageCalculationResult.Value;
    //}

    //private static (decimal gross, decimal cost, decimal benefit) CalculatePlanDailyUsage(
    //    CustomerCalculationDto customer,
    //    BenefitCalculationDto? planBenefit,
    //    DateTimeOffset utcNow)
    //{
    //    int dayCount = utcNow.Day == 1
    //        ? DateTime.DaysInMonth(utcNow.Year, utcNow.Month - 1)
    //        : DateTime.DaysInMonth(utcNow.Year, utcNow.Month);

    //    decimal price = customer.Plan.Price is null
    //        ? customer.Plan.Plan.Price
    //        : customer.Plan.Price.Value;

    //    decimal gross = price / dayCount * customer.Cars.Count;

    //    if (planBenefit is null)
    //    {
    //        return (gross, gross, 0m);
    //    }

    //    decimal cost;

    //    if (planBenefit.PaymentCategories.All(paymentCategory => paymentCategory.AmountType == AmountType.Fixed))
    //    {
    //        cost = (price - planBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount)) / dayCount;
    //    }
    //    else if (planBenefit.PaymentCategories.All(paymentCategory => paymentCategory.AmountType == AmountType.Percent))
    //    {
    //        cost = (MaxPercent - planBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount)) / MaxPercent / dayCount * price;
    //    }
    //    else
    //    {
    //        cost = price / dayCount;
    //    }

    //    return (gross, cost, gross - cost);
    //}

    //private List<DailyPackageUsageCalculation> CreatePackageDailyUsageCalculation(
    //    CustomerCalculationDto customer,
    //    List<BenefitCalculationDto> packageBenefits,
    //    Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries,
    //    List<CalculationException> exceptions,
    //    DateTimeOffset utcNow)
    //{
    //    List<DailyPackageUsageCalculation> packageDailyCharges = new(packageBenefits.Capacity / 2 + 10);
    //    bool unicUser = true;

    //    foreach (CustomerPackageCalculationDto CustomerPackage in customer.Plan.PlanPackages)
    //    {
    //        BenefitCalculationDto? packageBenefit = packageBenefits.FirstOrDefault(benefit => benefit.PaymentCategories.All(paymentCategory => paymentCategory.PackageId == CustomerPackage.PackageId));
    //        (decimal gross, decimal cost, decimal benefit) = CalculatePackageDailyUsage(CustomerPackage, packageBenefit, utcNow);
    //        CalculatePackageSummary(packageUsageSummaries, CustomerPackage.PackageId, gross, cost, unicUser, utcNow);
    //        unicUser = false;

    //        ErrorOr<DailyPackageUsageCalculation> dailyPackageUsageCalculationResult = DailyPackageUsageCalculation.Create(
    //            customerId: customer.Id,
    //            CustomerPackageId: CustomerPackage.Id,
    //            customerCarId: CustomerPackage.CustomerCarId,
    //            benefitId: packageBenefit?.Id ?? null,
    //            cost: cost);

    //        if (dailyPackageUsageCalculationResult.IsError)
    //        {
    //            IReadOnlyCollection<CalculationException> remoteExeptions = CalculationException.Create(
    //                dailyPackageUsageCalculationResult.Errors,
    //                type: CalculationExceptionType.Internal,
    //                dateTime: _timeProvider.GetUtcNow());

    //            exceptions.AddRange(remoteExeptions);
    //        }

    //        packageDailyCharges.Add(dailyPackageUsageCalculationResult.Value);
    //    }

    //    return packageDailyCharges;
    //}

    //private static (decimal gross, decimal cost, decimal benefit) CalculatePackageDailyUsage(
    //    CustomerPackageCalculationDto CustomerPackage,
    //    BenefitCalculationDto? benefit,
    //    DateTimeOffset utcNow)
    //{
    //    int dayCount = utcNow.Day == 1
    //        ? DateTime.DaysInMonth(utcNow.Year, utcNow.Month - 1)
    //        : DateTime.DaysInMonth(utcNow.Year, utcNow.Month);

    //    decimal gross = CustomerPackage.Price / dayCount;

    //    if (benefit is null)
    //    {
    //        return (gross, gross, 0m);
    //    }

    //    decimal cost;

    //    if (benefit.PaymentCategories.All(paymentCategory => paymentCategory.AmountType == AmountType.Fixed))
    //    {
    //        cost = (CustomerPackage.Price - benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount)) / dayCount;
    //    }
    //    else if (benefit.PaymentCategories.All(paymentCategory => paymentCategory.AmountType == AmountType.Percent))
    //    {
    //        cost = (MaxPercent - benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount)) / MaxPercent / dayCount * CustomerPackage.Price;
    //    }
    //    else
    //    {
    //        cost = CustomerPackage.Price;
    //    }

    //    return (gross, cost, gross - cost);
    //}

    //private List<DailyItemUsageCalculation> CreateItemDailyUsageCalculation(
    //    CustomerCalculationDto customer,
    //    List<BenefitCalculationDto> itemBenefits,
    //    Dictionary<Guid, DailyItemUsageSummary> dailyItemUsageSummaries,
    //    List<CustomerBenefitLimitCalculationDto> updatedCustomerBenefits,
    //    List<CalculationException> exceptions,
    //    DateTimeOffset utcNow)
    //{
    //    List<DailyItemUsageCalculation> itemDailyCharges = new(itemBenefits.Capacity / 2 + 10);
    //    bool unicUser = true;

    //    foreach (CustomerDailyChargeCalculationDto dailyCharge in customer.DailyCharges)
    //    {
    //        BenefitCalculationDto? itemBenefit = itemBenefits
    //            .FirstOrDefault(benefit => itemBenefits.Any(itemBenefit => benefit.PaymentCategories.All(paymentCategory => paymentCategory.ItemId == itemBenefit.Id)));

    //        CustomerBenefitCalculationDto? customerBenefit = itemBenefit is null
    //            ? null
    //            : customer.Benefits.FirstOrDefault(benefit => benefit.BenefitId == itemBenefit.Id);

    //        (decimal gross, decimal cost, decimal benefit) = CalculateItemDailyUsage(customerBenefit, itemBenefit, dailyCharge, updatedCustomerBenefits);
    //        CalculateItemSummary(dailyItemUsageSummaries, dailyCharge.ItemId, gross, cost, unicUser, utcNow);
    //        unicUser = false;

    //        ErrorOr<DailyItemUsageCalculation> dailyItemUsageCalculationResult = DailyItemUsageCalculation.Create(
    //            customerId: customer.Id,
    //            benefitId: itemBenefit?.Id ?? null,
    //            cost: cost);

    //        if (dailyItemUsageCalculationResult.IsError)
    //        {
    //            IReadOnlyCollection<CalculationException> remoteExeptions = CalculationException.Create(
    //                dailyItemUsageCalculationResult.Errors,
    //                type: CalculationExceptionType.Internal,
    //                dateTime: _timeProvider.GetUtcNow());

    //            exceptions.AddRange(remoteExeptions);
    //        }

    //        itemDailyCharges.Add(dailyItemUsageCalculationResult.Value);
    //    }

    //    return itemDailyCharges;
    //}

    //private static (decimal gross, decimal cost, decimal benefit) CalculateItemDailyUsage(
    //    CustomerBenefitCalculationDto? customerBenefit,
    //    BenefitCalculationDto? itemBenefit,
    //    CustomerDailyChargeCalculationDto dailyCharge,
    //    List<CustomerBenefitLimitCalculationDto> updatedCustomerBenefits)
    //{
    //    decimal itemPrice = dailyCharge.Price;
    //    decimal gross = dailyCharge.Count * itemPrice;
    //    decimal cost = gross;

    //    if (itemBenefit is not null)
    //    {
    //        decimal fixedPrice = itemPrice - itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount);
    //        BenefitPaymentCategoryCalculationDto benefitPaymentCategory = itemBenefit.PaymentCategories.First();

    //        switch (benefitPaymentCategory.FrequencyType.Key)
    //        {
    //            case 3:
    //                {
    //                    cost = benefitPaymentCategory.AmountType.Key switch
    //                    {
    //                        1 => (fixedPrice > 0 ? fixedPrice : 0) * dailyCharge.Count,
    //                        2 => (MaxPercent - itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount)) / MaxPercent * dailyCharge.Count * itemPrice,
    //                        _ => dailyCharge.Count * itemPrice,
    //                    };
    //                }
    //                break;
    //            case 1:
    //                {
    //                    int withBenefit = dailyCharge.Count > customerBenefit!.FrequencyCountLeft!.Value
    //                        ? customerBenefit.FrequencyCountLeft!.Value
    //                        : dailyCharge.Count;

    //                    int withoutBenefit = dailyCharge.Count - customerBenefit!.FrequencyCountLeft!.Value < 0
    //                        ? 0
    //                        : dailyCharge.Count - customerBenefit!.FrequencyCountLeft!.Value;

    //                    cost = benefitPaymentCategory.AmountType.Key switch
    //                    {
    //                        1 => (fixedPrice > 0 ? fixedPrice : 0) * withBenefit + itemPrice * withoutBenefit,
    //                        2 => (MaxPercent - itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount) / MaxPercent * dailyCharge.Count) * itemPrice,
    //                        _ => dailyCharge.Count * itemPrice,
    //                    };

    //                    // TODO
    //                    //customerBenefit.UpdateFrequencyCountLeft(
    //                    //customerBenefit.FrequencyCountLeft!.Value - withBenefit < 0
    //                    //    ? 0
    //                    //    : customerBenefit.FrequencyCountLeft!.Value - withBenefit);

    //                    int frequencyCountLeft = customerBenefit.FrequencyCountLeft!.Value - withBenefit < 0
    //                        ? 0
    //                        : customerBenefit.FrequencyCountLeft!.Value - withBenefit;

    //                    decimal? limitAmountLeft = customerBenefit.LimitAmountLeft is null
    //                        ? null
    //                        : customerBenefit.LimitAmountLeft.Value;

    //                    CustomerBenefitLimitCalculationDto dto = new()
    //                    {
    //                        Id = customerBenefit.Id,
    //                        FrequencyCountLeft = frequencyCountLeft,
    //                        LimitAmountLeft = limitAmountLeft,
    //                    };

    //                    updatedCustomerBenefits.Add(dto);
    //                }
    //                break;
    //            case 2:
    //                {
    //                    int withBenefit = (customerBenefit!.FrequencyCountLeft!.Value + dailyCharge.Count) % (itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Frequency) ?? 1);

    //                    int withoutBenefit = dailyCharge.Count - withBenefit;

    //                    cost = benefitPaymentCategory.AmountType.Key switch
    //                    {
    //                        1 => (fixedPrice > 0 ? fixedPrice : 0) * withBenefit + itemPrice * withoutBenefit,
    //                        2 => (MaxPercent - itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount) / MaxPercent * dailyCharge.Count) * itemPrice,
    //                        _ => dailyCharge.Count * itemPrice,
    //                    };

    //                    // TODO
    //                    //customerBenefit.UpdateFrequencyCountLeft(
    //                    //  customerBenefit!.FrequencyCountLeft!.Value + dailyCharge.Count - withBenefit * (itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Frequency)
    //                    //  ?? 1));

    //                    int frequencyCountLeft = customerBenefit!.FrequencyCountLeft!.Value + dailyCharge.Count - withBenefit * (itemBenefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Frequency) ?? 1);

    //                    decimal? limitAmountLeft = customerBenefit.LimitAmountLeft is null
    //                        ? null
    //                        : customerBenefit.LimitAmountLeft.Value;

    //                    CustomerBenefitLimitCalculationDto dto = new()
    //                    {
    //                        Id = customerBenefit.Id,
    //                        FrequencyCountLeft = frequencyCountLeft,
    //                        LimitAmountLeft = limitAmountLeft,
    //                    };

    //                    updatedCustomerBenefits.Add(dto);
    //                }
    //                break;
    //            default:
    //                {
    //                    cost = gross < customerBenefit!.LimitAmountLeft!.Value
    //                        ? 0
    //                        : gross - customerBenefit!.LimitAmountLeft!.Value;

    //                    // TODO
    //                    //customerBenefit.UpdateLimitAmountLeft(
    //                    //  gross < customerBenefit!.LimitAmountLeft!.Value
    //                    //  ? customerBenefit!.LimitAmountLeft!.Value - gross
    //                    //  : 0);

    //                    int frequencyCountLeft = customerBenefit.FrequencyCountLeft is null
    //                        ? 0
    //                        : customerBenefit.FrequencyCountLeft.Value;

    //                    decimal? limitAmountLeft = gross < customerBenefit!.LimitAmountLeft!.Value
    //                        ? customerBenefit!.LimitAmountLeft!.Value - gross
    //                        : 0.0m;

    //                    CustomerBenefitLimitCalculationDto dto = new()
    //                    {
    //                        Id = customerBenefit.Id,
    //                        FrequencyCountLeft = frequencyCountLeft,
    //                        LimitAmountLeft = limitAmountLeft,
    //                    };
    //                }
    //                break;
    //        }
    //    }

    //    return (gross, gross, gross - cost);
    //}

    //private static void CalculateItemSummary(
    //    Dictionary<Guid, DailyItemUsageSummary> itemUsageSummaries,
    //    Guid itemId,
    //    decimal gross,
    //    decimal cost,
    //    bool unicUser,
    //    DateTimeOffset utcNow)
    //{
    //    bool exists = itemUsageSummaries.TryGetValue(itemId, out DailyItemUsageSummary? itemUsageSummary);

    //    if (!exists || itemUsageSummary is null)
    //    {
    //        itemUsageSummary = DailyItemUsageSummary.Create(
    //            itemId: itemId,
    //            date: DateOnly.FromDateTime(utcNow.UtcDateTime),
    //            gross: 0m,
    //            cost: 0m,
    //            benefitCost: 0m,
    //            benefitQuantity: 0,
    //            quantity: 0,
    //            customerCount: 0).Value;

    //        itemUsageSummaries.Add(itemUsageSummary.ItemId, itemUsageSummary);
    //    }

    //    itemUsageSummary.UpdateRercord(gross, cost, unicUser);
    //}

    //private static void CalculatePlanSummary(
    //    Dictionary<Guid, DailyPlanUsageSummary> planUsageSummaries,
    //    Guid planId,
    //    decimal gross,
    //    decimal cost,
    //    DateTimeOffset utcNow)
    //{
    //    bool exists = planUsageSummaries.TryGetValue(planId, out DailyPlanUsageSummary? planUsageSummary);

    //    if (!exists || planUsageSummary is null)
    //    {
    //        planUsageSummary = DailyPlanUsageSummary.Create(
    //            planId: planId,
    //            date: DateOnly.FromDateTime(utcNow.UtcDateTime),
    //            gross: 0m,
    //            cost: 0m,
    //            benefitCost: 0m,
    //            benefitQuantity: 0,
    //            quantity: 0,
    //            customerCount: 0).Value;

    //        planUsageSummaries.Add(planUsageSummary.PlanId, planUsageSummary);
    //    }

    //    planUsageSummary.UpdateRercord(gross, cost);
    //}

    //private static void CalculatePackageSummary(
    //    Dictionary<Guid, DailyPackageUsageSummary> packageUsageSummaries,
    //    Guid packageId,
    //    decimal gross,
    //    decimal cost,
    //    bool unicUser,
    //    DateTimeOffset utcNow)
    //{
    //    bool exists = packageUsageSummaries.TryGetValue(packageId, out DailyPackageUsageSummary? packageUsageSummary);

    //    if (!exists || packageUsageSummary is null)
    //    {
    //        packageUsageSummary = DailyPackageUsageSummary.Create(
    //            packageId: packageId,
    //            date: DateOnly.FromDateTime(utcNow.UtcDateTime),
    //            gross: 0m,
    //            cost: 0m,
    //            benefitCost: 0m,
    //            benefitQuantity: 0,
    //            quantity: 0,
    //            customerCount: 0).Value;

    //        packageUsageSummaries.Add(packageUsageSummary.PackageId, packageUsageSummary);
    //    }

    //    packageUsageSummary.UpdateRercord(gross, cost, unicUser);
    //}
}
