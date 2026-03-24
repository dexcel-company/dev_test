using CelloPark.Application.Common.Contexts;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Domain.Features.Benefits.Entities.BenefitCoupons;
using CelloPark.Domain.Features.Benefits.Entities.BenefitPaymentCategories;
using CelloPark.Domain.Features.Customers;
using CelloPark.Domain.Features.Customers.Entities.CustomerBenefit;
using CelloPark.Domain.Features.Customers.Entities.CustomerCars;
using CelloPark.Domain.Features.Customers.Entities.CustomerCouponUsages;
using CelloPark.Domain.Features.Customers.Entities.CustomerCredits;
using CelloPark.Domain.Features.Customers.Entities.CustomerPackages;
using CelloPark.Domain.Features.Customers.Entities.CustomerPlans;
using CelloPark.Domain.Features.DailyItemUsageSummaries;
using CelloPark.Domain.Features.DailyPackageUsageSummaries;
using CelloPark.Domain.Features.DailyPlanUsageSummaries;
using CelloPark.Domain.Features.Items;
using CelloPark.Domain.Features.Packages;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Domain.Features.Plans;
using CelloPark.Domain.Features.Roles;
using CelloPark.Domain.Features.Users;
using CelloPark.Domain.Features.Users.Entities.RefreshSessions;
using CelloPark.Infrastructure.Common.Environments.Constants;
using CelloPark.Infrastructure.Common.Interceptors;
using CelloPark.Infrastructure.Common.Providers;
using Microsoft.EntityFrameworkCore;

namespace CelloPark.Infrastructure.Common.Contexts;

internal class ManagementContext :
    DbContext, IManagementContext
{
    public ManagementContext(
        DbContextOptions<ManagementContext> options,
        CreateDetailsInterceptor createDetailInterceptor,
        UpdateDetailsInterceptor updateDetailsInterceptor,
        DeleteDetailsInterceptor deleteDetailsInterceptor,
        ShadowIdInterceptor shadowIdInterceptor) : base(options)
    {
        _createDetailsInterceptor = createDetailInterceptor;
        _updateDetailsInterceptor = updateDetailsInterceptor;
        _deleteDetailsInterceptor = deleteDetailsInterceptor;
        _shadowIdInterceptor = shadowIdInterceptor;
    }

    public DbSet<User> Users { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<RefreshSession> RefreshSessions { get; init; }
    public DbSet<Item> Items { get; init; }
    public DbSet<Plan> Plans { get; init; }
    public DbSet<Package> Packages { get; init; }
    public DbSet<PlanPackage> PlanPackages { get; init; }
    public DbSet<Benefit> Benefits { get; init; }
    public DbSet<BenefitPaymentCategory> BenefitPaymentCategories { get; init; }
    public DbSet<Customer> Customers { get; init; }
    public DbSet<CustomerCar> CustomerCars { get; init; }
    public DbSet<CustomerPlan> CustomerPlans { get; init; }
    public DbSet<CustomerPackage> CustomerPackages { get; init; }
    public DbSet<CustomerBenefit> CustomerBenefits { get; init; }
    public DbSet<CustomerCredit> CustomerCredits { get; init; }
    public DbSet<CustomerCouponUsage> CustomerCouponUsages { get; init; }
    public DbSet<DailyPlanUsageSummary> DailyPlanUsageSummaries { get; init; }
    public DbSet<DailyPackageUsageSummary> DailyPackageUsageSummaries { get; init; }
    public DbSet<DailyItemUsageSummary> DailyItemUsageSummaries { get; init; }

    private readonly CreateDetailsInterceptor _createDetailsInterceptor;
    private readonly UpdateDetailsInterceptor _updateDetailsInterceptor;
    private readonly DeleteDetailsInterceptor _deleteDetailsInterceptor;
    private readonly ShadowIdInterceptor _shadowIdInterceptor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(DatabaseProvider.Schema)
            .ApplyConfigurationsFromAssembly(typeof(ManagementContext).Assembly);

        modelBuilder
            .Entity<Item>()
            .HasQueryFilter(item => item.Status != Status.Deleted);

        modelBuilder
            .Entity<Plan>()
            .HasQueryFilter(plan => plan.Status != Status.Deleted);

        modelBuilder
            .Entity<Package>()
            .HasQueryFilter(package => package.Status != Status.Deleted);

        modelBuilder
            .Entity<PlanPackage>()
            .HasQueryFilter(planPackage => planPackage.Status != Status.Deleted);

        modelBuilder
            .Entity<Benefit>()
            .HasQueryFilter(benefit => benefit.Status != Status.Deleted);

        modelBuilder
            .Entity<BenefitPaymentCategory>()
            .HasQueryFilter(benefitPaymentCategory => benefitPaymentCategory.Status != Status.Deleted);

        modelBuilder
            .Entity<BenefitCoupon>()
            .HasQueryFilter(benefitCoupon => benefitCoupon.Status != Status.Deleted);

        modelBuilder
            .Entity<Customer>()
            .HasQueryFilter(customer => customer.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerCar>()
            .HasQueryFilter(customerCar => customerCar.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerCredit>()
            .HasQueryFilter(customerCredit => customerCredit.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerPlan>()
            .HasQueryFilter(customerPlan => customerPlan.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerPackage>()
            .HasQueryFilter(CustomerPackage => CustomerPackage.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerBenefit>()
            .HasQueryFilter(customerBenefit => customerBenefit.Status != Status.Deleted);

        modelBuilder
            .Entity<CustomerCouponUsage>()
            .HasQueryFilter(customerCouponUsage => customerCouponUsage.Status != Status.Deleted);

        modelBuilder
            .Entity<DailyPlanUsageSummary>()
            .HasQueryFilter(dailyPlanUsageSummary => dailyPlanUsageSummary.Status != Status.Deleted);

        modelBuilder
            .Entity<DailyPackageUsageSummary>()
            .HasQueryFilter(dailyPackageUsageSummary => dailyPackageUsageSummary.Status != Status.Deleted);

        modelBuilder
            .Entity<DailyItemUsageSummary>()
            .HasQueryFilter(dailyItemUsageSummary => dailyItemUsageSummary.Status != Status.Deleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(DatabaseProvider.ConnectionString, serverOptions =>
            {
                string? commandTimeout = Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout);

                serverOptions.CommandTimeout(Convert.ToInt32(commandTimeout));
            })
            .AddInterceptors(_createDetailsInterceptor)
            .AddInterceptors(_updateDetailsInterceptor)
            .AddInterceptors(_deleteDetailsInterceptor)
            .AddInterceptors(_shadowIdInterceptor);
    }
}
