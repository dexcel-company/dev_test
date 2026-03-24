using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Domain.Features.CalculationExceptions;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;
using CelloPark.Domain.Features.Customers.Entities.CustomerCredits;
using CelloPark.Domain.Features.Customers.Entities.CustomerDailyCharges;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Domain.Features.Plans;
using CelloPark.Infrastructure.Common.Environments.Constants;
using CelloPark.Infrastructure.Common.Providers;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Contexts;

internal sealed class DailyUsageContext :
    DbContext
{
    public DbSet<Item> Items { get; init; }
    public DbSet<Plan> Plans { get; init; }
    public DbSet<Package> Packages { get; init; }
    public DbSet<PlanPackage> PlanPackages { get; init; }
    public DbSet<Benefit> Benefits { get; init; }
    public DbSet<BenefitPaymentCategory> BenefitPaymentCategories { get; init; }
    public DbSet<BenefitCoupon> BenefitCoupons { get; init; }
    public DbSet<Customer> Customers { get; init; }
    public DbSet<CustomerCar> CustomerCars { get; init; }
    public DbSet<CustomerPlan> CustomerPlans { get; init; }
    public DbSet<CustomerPackage> CustomerPackages { get; init; }
    public DbSet<CustomerBenefit> CustomerBenefits { get; init; }
    public DbSet<CustomerCredit> CustomerCredits { get; init; }
    public DbSet<CustomerCouponUsage> CustomerCouponUsages { get; init; }
    public DbSet<CustomerDailyCharge> DailyCharges { get; init; }
    public DbSet<DailyItemUsageSummary> DailyItemUsageSummaries { get; init; }
    public DbSet<DailyPlanUsageSummary> DailyPlanUsageSummaries { get; init; }
    public DbSet<DailyPackageUsageSummary> DailyPackageUsageSummaries { get; init; }
    public DbSet<BenefitSnapshot> BenefitSnapshots { get; init; }
    public DbSet<BenefitPaymentCategorySnapshot> BenefitPaymentCategorySnapshots { get; init; }
    public DbSet<BenefitCouponSnapshot> BenefitCouponSnapshots { get; init; }
    public DbSet<CustomerSnapshot> CustomerSnapshots { get; init; }
    public DbSet<CustomerPlanSnapshot> CustomerPlanSnapshots { get; init; }
    public DbSet<CustomerPackageSnapshot> CustomerPackageSnapshots { get; init; }
    public DbSet<CustomerCarSnapshot> CustomerCarSnapshots { get; init; }
    public DbSet<CustomerBenefitSnapshot> CustomerBenefitSnapshots { get; init; }
    public DbSet<CustomerDailyChargeSnapshot> DailyChargeSnapshots { get; init; }
    public DbSet<CalculationException> CalculationExceptions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(DatabaseProvider.Schema)
            .ApplyConfigurationsFromAssembly(typeof(DailyUsageContext).Assembly);

        modelBuilder
            .Entity<Item>()
            .HasQueryFilter(item => item.Status == Status.Active);

        modelBuilder
            .Entity<Plan>()
            .HasQueryFilter(plan => plan.Status == Status.Active);

        modelBuilder
            .Entity<Package>()
            .HasQueryFilter(package => package.Status == Status.Active);

        modelBuilder
            .Entity<PlanPackage>()
            .HasQueryFilter(planPackage => planPackage.Status == Status.Active);

        modelBuilder
            .Entity<Benefit>()
            .HasQueryFilter(benefit => benefit.Status == Status.Active);

        modelBuilder
            .Entity<BenefitPaymentCategory>()
            .HasQueryFilter(benefitPaymentCategory => benefitPaymentCategory.Status == Status.Active);

        modelBuilder
            .Entity<BenefitCoupon>()
            .HasQueryFilter(benefitCoupon => benefitCoupon.Status == Status.Active);

        modelBuilder
            .Entity<Customer>()
            .HasQueryFilter(customer => customer.Status == Status.Active);

        modelBuilder
            .Entity<CustomerCar>()
            .HasQueryFilter(customerCar => customerCar.Status == Status.Active);

        modelBuilder
            .Entity<CustomerPlan>()
            .HasQueryFilter(customerPlan => customerPlan.Status == Status.Active);

        modelBuilder
            .Entity<CustomerPackage>()
            .HasQueryFilter(CustomerPackage => CustomerPackage.Status == Status.Active);

        modelBuilder
            .Entity<CustomerBenefit>()
            .HasQueryFilter(customerBenefit => customerBenefit.Status == Status.Active);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(DatabaseProvider.ConnectionString, serverOptions =>
            {
                string? commandTimeout = Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout);

                serverOptions.CommandTimeout(Convert.ToInt32(commandTimeout));
            })
            .EnableThreadSafetyChecks(false)
            .EnableDetailedErrors(false)
            .EnableSensitiveDataLogging(false);
        //.LogTo(Console.WriteLine, LogLevel.Information);
    }
}
