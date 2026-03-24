namespace CelloPark.Infrastructure.Common.Environments.Options;

internal sealed class EnvironmentOptions
{
    public string AspNetCoreEnvironment { get; init; } = null!;
    public string DatabaseHost { get; init; } = null!;
    public string DatabasePort { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
    public string DatabaseUsername { get; init; } = null!;
    public string DatabasePassword { get; init; } = null!;
    public string DatabaseSchema { get; init; } = null!;
    public string DatabaseIntegratedSecurity { get; init; } = null!;
    public string DatabaseTrustServerCertificate { get; init; } = null!;
    public string DatabaseEncrypt { get; init; } = null!;
    public string DatabaseCommandTimeout { get; init; } = null!;
    public string AccessTokenSecret { get; init; } = null!;
    public string AccessTokenExpiresIn { get; init; } = null!;
    public string AllowedOrigins { get; init; } = null!;
    public string BackgroundJobCron { get; init; } = null!;
    public string BackgroundJobBatchSize { get; init; } = null!;
    public string BackgroundJobThreadCount { get; init; } = null!;
}
