using CelloPark.Application.Common.Contexts;
using CelloPark.Application.Common.Passwords.Hashers.Abstractions;
using CelloPark.Application.Common.Tokens.Generators.Abstractions;
using CelloPark.Application.Features.Benefits.Exports.Abstractions;
using CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;
using CelloPark.Application.Features.Packets.Services.Abstractions;
using CelloPark.Application.Features.Plans.Services.Abstractions;
using CelloPark.Application.Features.Users.ActionContexts.Abstractions;
using CelloPark.Infrastructure.Common.ActionContexts;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.CalculationWorkers.Abstractions;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ExtractionWorkers.Abstractions;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers;
using CelloPark.Infrastructure.Common.BackgroundJobs.DailyUsages.ShapshotWorkers.Abstractions;
using CelloPark.Infrastructure.Common.Contexts;
using CelloPark.Infrastructure.Common.Environments.Constants;
using CelloPark.Infrastructure.Common.Environments.Options;
using CelloPark.Infrastructure.Common.Environments.Validators;
using CelloPark.Infrastructure.Common.Interceptors;
using CelloPark.Infrastructure.Common.Passwords.Hashers;
using CelloPark.Infrastructure.Common.Tokens.Generators;
using CelloPark.Infrastructure.Common.Tokens.Options;
using CelloPark.Infrastructure.Features.Benefits.Exports;
using CelloPark.Infrastructure.Features.DailyUsageSummaries.Services;
using CelloPark.Infrastructure.Features.Packets.Services;
using CelloPark.Infrastructure.Features.Plans.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;

namespace CelloPark.Infrastructure.Common.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string corsPolicyName)
    {
        services
            .AddEnvironment()
            .AddHttpContextAccessor()
            .AddDatabase()
            .AddServices()
            .AddSecurity()
            .AddCors(corsPolicyName)
            .AddBackgroundJobs()
            .AddBackgroundJobWorkers();

        return services;
    }

    private static IServiceCollection AddEnvironment(this IServiceCollection services)
    {
        const char Separator = '=';
        const int SplitCount = 2;
        const int Key = 0;
        const int Value = 1;

        if (!System.Diagnostics.Debugger.IsAttached)
        {
            ValidateEnvironmentVariables();

            return services;
        }

        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), $".env.{environment}");

        if (!File.Exists(filePath))
        {
            throw new Exception($"Unable to read environment variables from {filePath}. File not found.");
        }

        string[] fileContent = File.ReadAllLines(filePath);

        foreach (string line in fileContent)
        {
            string[] parts = line.Split(Separator, SplitCount, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == SplitCount)
            {
                Environment.SetEnvironmentVariable(parts[Key], parts[Value]);
            }
        }

        ValidateEnvironmentVariables();

        return services;
    }

    private static void ValidateEnvironmentVariables()
    {
        EnvironmentOptions environmentOptions = new()
        {
            AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!,
            DatabaseHost = Environment.GetEnvironmentVariable(DatabaseKeys.Host)!,
            DatabasePort = Environment.GetEnvironmentVariable(DatabaseKeys.Port)!,
            DatabaseName = Environment.GetEnvironmentVariable(DatabaseKeys.Name)!,
            DatabaseUsername = Environment.GetEnvironmentVariable(DatabaseKeys.Username)!,
            DatabasePassword = Environment.GetEnvironmentVariable(DatabaseKeys.Password)!,
            DatabaseSchema = Environment.GetEnvironmentVariable(DatabaseKeys.Schema)!,
            DatabaseIntegratedSecurity = Environment.GetEnvironmentVariable(DatabaseKeys.IntegratedSecurity)!,
            DatabaseTrustServerCertificate = Environment.GetEnvironmentVariable(DatabaseKeys.TrustServerCertificate)!,
            DatabaseEncrypt = Environment.GetEnvironmentVariable(DatabaseKeys.Encrypt)!,
            DatabaseCommandTimeout = Environment.GetEnvironmentVariable(DatabaseKeys.CommandTimeout)!,
            AccessTokenSecret = Environment.GetEnvironmentVariable(AccessTokenKeys.Secret)!,
            AccessTokenExpiresIn = Environment.GetEnvironmentVariable(AccessTokenKeys.ExpiresIn)!,
            AllowedOrigins = Environment.GetEnvironmentVariable(CorsKeys.AllowedOrigins)!,
            BackgroundJobCron = Environment.GetEnvironmentVariable(BackgroundJobKeys.Cron)!,
            BackgroundJobBatchSize = Environment.GetEnvironmentVariable(BackgroundJobKeys.BatchSize)!,
            BackgroundJobThreadCount = Environment.GetEnvironmentVariable(BackgroundJobKeys.ThreadCount)!,
        };

        EnvironmentOptionsValidator validator = new();
        validator.ValidateAndThrow(environmentOptions);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services
            .AddScoped<CreateDetailsInterceptor>()
            .AddScoped<UpdateDetailsInterceptor>()
            .AddScoped<DeleteDetailsInterceptor>()
            .AddScoped<ShadowIdInterceptor>()
            .AddDbContext<IManagementContext, ManagementContext>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>()
            .AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>()
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddSingleton<IBenefitExportService, BenefitExportService>()
            .AddSingleton<IPlanExportService, PlanExportService>()
            .AddSingleton<IPackageExportService, PackageExportService>()
            .AddSingleton<IDailyUsageSummariesExportService, DailyUsageSummariesExportService>()
            .AddScoped<IUserActionContext, UserActionContext>()
            .AddScoped<ICalculationService, CalculationService>();

        return services;
    }

    private static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        string secret = Environment.GetEnvironmentVariable(AccessTokenKeys.Secret)!;
        string expiresIn = Environment.GetEnvironmentVariable(AccessTokenKeys.ExpiresIn)!;

        services.AddAuthentication(options =>
         {
             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
         })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            };
        });

        services.AddAuthorization();

        IOptions<AccessTokenOptions> accessTokenOptions = Options.Create(new AccessTokenOptions
        {
            Secret = secret,
            ExpiresIn = Convert.ToInt64(expiresIn),
        });

        services.AddSingleton(accessTokenOptions);

        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services, string CorsPolicyName)
    {
        const char Separator = ',';

        services.AddCors(options =>
        {
            string allowedOriginsString = Environment.GetEnvironmentVariable(CorsKeys.AllowedOrigins)!;
            string[] allowedOrigins = allowedOriginsString.Split(Separator, StringSplitOptions.TrimEntries);

            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy = policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(allowedOrigins)
                    .SetIsOriginAllowed(_ => true);
            });
        });

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        string cron = Environment.GetEnvironmentVariable(BackgroundJobKeys.Cron)!;

        cron = cron.Replace('_', ' ');

        services.AddQuartz(configure =>
        {
            configure.AddJob<DailyUsageJob>(job => job
                .WithIdentity(DailyUsageJob.Key));

            configure.AddTrigger(trigger => trigger
                .ForJob(DailyUsageJob.Key)
                .WithIdentity("DailyUsagesJob-trigger")
                .WithCronSchedule(cron, options =>
                {
                    options.InTimeZone(TimeZoneInfo.Utc);
                }));
        });

        services.AddQuartzHostedService(configure =>
        {
            configure.WaitForJobsToComplete = true;
        });

        return services;
    }

    private static IServiceCollection AddBackgroundJobWorkers(this IServiceCollection services)
    {
        services.AddSingleton<ISnapshotWorker, SnapshotWorker>();
        services.AddSingleton<ICalculationWorker, CalculationWorker>();
        services.AddSingleton<IExtractionWorker, ExtractionWorker>();

        return services;
    }
}
