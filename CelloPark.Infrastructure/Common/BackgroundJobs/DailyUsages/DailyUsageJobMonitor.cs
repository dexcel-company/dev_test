namespace CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages;

internal static class DailyUsageJobMonitor
{
    public static bool IsRunning { get; private set; }

    public static void Lock()
    {
        IsRunning = true;
    }

    public static void Unlock()
    {
        IsRunning = false;
    }
}