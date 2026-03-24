namespace CelloPark.Infrastructure.Common.Contexts.Constants;

internal static class DatabaseContextTableNames
{
    public const string Users = "User";
    public const string Roles = "Role";
    public const string RefreshSessions = "RefreshSession";

    public const string Items = "Item";
    public const string Plans = "Plan";
    public const string Packages = "Package";
    public const string PlanPackages = "PlanPackage";
    public const string LimitDiscountTypes = "LimitDiscountType";
    public const string Benefits = "Benefit";
    public const string BenefitPaymentCategories = "BenefitPaymentCategory";
    public const string BenefitCoupons = "BenefitCoupon";

    public const string Customers = "Customer";
    public const string CustomerViews = "CustomerView";
    public const string CustomerCars = "CustomerCar";
    public const string CustomerPlans = "CustomerPlan";
    public const string CustomerPackages = "CustomerPackage";
    public const string CustomerBenefits = "CustomerBenefit";
    public const string CustomerCredits = "CustomerCredit";
    public const string CustomerCouponUsages = "CustomerCouponUsage";

    public const string DailyCharges = "DailyCharges";
    public const string MonthlyCharges = "MonthlyCharges";
    public const string DailyPlanUsageCalculations = "DailyPlanUsageCalculations";
    public const string DailyPackageUsageCalculations = "DailyPackageUsageCalculations";
    public const string DailyItemUsageCalculations = "DailyItemUsageCalculations";
    public const string DailyPlanUsageSummaries = "DailyPlanUsageSummary";
    public const string DailyPackageUsageSummaries = "DailyPackageUsageSummary";
    public const string DailyItemUsageSummaries = "DailyItemUsageSummary";

    public const string RemoteCustomers = "Users";
    public const string RemoteCustomerCars = "UserCars";
    public const string RemoteCustomerCredits = "UserCredits";
    public const string RemoteCustomerPackages = "UserPackages";
    public const string RemoteDailyCharges = "DailyItemsUsage";
    public const string CalculationExceptions = "CalculationException";
};
