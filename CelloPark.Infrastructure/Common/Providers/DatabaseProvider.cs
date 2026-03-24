using CelloPark.Infrastructure.Common.Environments.Constants;

namespace CelloPark.Infrastructure.Common.Providers;

internal static class DatabaseProvider
{
    static DatabaseProvider()
    {
        if (System.Diagnostics.Debugger.IsAttached && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            ConnectionString = $"""
                Server={Environment.GetEnvironmentVariable(DatabaseKeys.Host)};
                Database={Environment.GetEnvironmentVariable(DatabaseKeys.Name)};
                Integrated Security={Environment.GetEnvironmentVariable(DatabaseKeys.IntegratedSecurity)};
            """;
        }
        else
        {
            ConnectionString = $"""
                Server={Environment.GetEnvironmentVariable(DatabaseKeys.Host)},{Environment.GetEnvironmentVariable(DatabaseKeys.Port)};
                Database={Environment.GetEnvironmentVariable(DatabaseKeys.Name)};
                User={Environment.GetEnvironmentVariable(DatabaseKeys.Username)};
                Password={Environment.GetEnvironmentVariable(DatabaseKeys.Password)};
                Integrated Security={Environment.GetEnvironmentVariable(DatabaseKeys.IntegratedSecurity)};
                TrustServerCertificate={Environment.GetEnvironmentVariable(DatabaseKeys.TrustServerCertificate)};
                Encrypt={Environment.GetEnvironmentVariable(DatabaseKeys.Encrypt)};
            """;
        }

        Schema = Environment.GetEnvironmentVariable(DatabaseKeys.Schema)!;
    }

    public static readonly string ConnectionString;
    public static readonly string Schema;
}
