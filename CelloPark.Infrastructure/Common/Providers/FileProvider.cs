namespace CelloPark.Infrastructure.Common.Providers;

internal static class FileProvider
{
    public static readonly string ExportFilesDirectory = Path.Combine(Environment.CurrentDirectory, "ExportFiles");
    public static readonly string ExceptionFilesDirectory = Path.Combine(Environment.CurrentDirectory, "ExceptionFiles");
}
