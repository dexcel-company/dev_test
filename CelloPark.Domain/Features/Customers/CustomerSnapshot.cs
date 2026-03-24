using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;
using CelloPark.Domain.Features.Customers.Entities.CustomerCredits;
using CelloPark.Domain.Features.Customers.Entities.CustomerDailyCharges;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;

namespace CelloPark.Domain.Features.Customers;

public sealed class CustomerSnapshot
{
    public string Id { get; } = null!;
    public string Name { get; } = null!;
    public ContractType ContractType { get; } = null!;
    public long PlanId { get; }
    public CustomerPlanSnapshot Plan { get; } = null!;
    public DateOnly SnapshotDate { get; }
    public IReadOnlyList<CustomerCarSnapshot> Cars => _cars.AsReadOnly();
    public IReadOnlyList<CustomerBenefitSnapshot> Benefits => _benefits.AsReadOnly();
    public IReadOnlyList<CustomerCreditSnapshot> Credits => _credits.AsReadOnly();
    public IReadOnlyList<CustomerCouponUsageSnapshot> CouponUsages => _couponUsages.AsReadOnly();
    public IReadOnlyList<CustomerDailyChargeSnapshot> DailyCharges => _dailyCharges.AsReadOnly();
    public IReadOnlyList<CustomerPackageSnapshot> PlanPackages => _customerPackages.AsReadOnly();

    private readonly List<CustomerCarSnapshot> _cars = [];
    private readonly List<CustomerBenefitSnapshot> _benefits = [];
    private readonly List<CustomerCreditSnapshot> _credits = [];
    private readonly List<CustomerCouponUsageSnapshot> _couponUsages = [];
    private readonly List<CustomerDailyChargeSnapshot> _dailyCharges = [];
    private readonly List<CustomerPackageSnapshot> _customerPackages = [];
}
