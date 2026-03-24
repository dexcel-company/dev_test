using CelloPark.Infrastructure.Common.Environments.Options;
using FluentValidation;

namespace CelloPark.Infrastructure.Common.Environments.Validators;

internal sealed class EnvironmentOptionsValidator :
    AbstractValidator<EnvironmentOptions>
{
    private const int AccessTokenSecretLength = 32;
    private const int AccessTokenExpiresInLength = 18;
    private const int BackgroundJobBatchSizeLength = 9;
    private const int BackgroundJobThreadCountLength = 9;

    public EnvironmentOptionsValidator()
    {
        RuleFor(property => property.AspNetCoreEnvironment)
            .NotEmpty()
            .WithMessage("'ASPNETCORE_ENVIRONMENT' variable must not be null, empty or white space.")
            .Must(value => string.Equals(value, "development", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "staging", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "production", StringComparison.OrdinalIgnoreCase))
            .WithMessage("'ASPNETCORE_ENVIRONMENT' variable must be 'Development', 'Staging' or 'Production'");

        RuleFor(property => property.DatabaseHost)
            .NotEmpty()
            .WithMessage("'DATABASE_HOST' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabasePort)
            .NotEmpty()
            .WithMessage("'DATABASE_PORT' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabaseName)
            .NotEmpty()
            .WithMessage("'DATABASE_NAME' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabaseUsername)
            .NotEmpty()
            .WithMessage("'DATABASE_USERNAME' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabasePassword)
            .NotEmpty()
            .WithMessage("'DATABASE_PASSWORD' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabaseSchema)
            .NotEmpty()
            .WithMessage("'DATABASE_SCHEMA' variable must not be null, empty or white space.");

        RuleFor(property => property.DatabaseIntegratedSecurity)
            .NotEmpty()
            .WithMessage("'DATABASE_INTEGRATED_SECURITY' variable must not be null, empty or white space.")
            .Must(value => string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
            .WithMessage("'DATABASE_INTEGRATED_SECURITY' variable must be 'True' or 'False'.");

        RuleFor(property => property.DatabaseTrustServerCertificate)
            .NotEmpty()
            .WithMessage("'DATABASE_TRUST_SERVER_CERTIFICATE' variable must not be null, empty or white space.")
            .Must(value => string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
            .WithMessage("'DATABASE_TRUST_SERVER_CERTIFICATE' variable must be 'True' or 'False'.");

        RuleFor(property => property.DatabaseEncrypt)
            .NotEmpty()
            .WithMessage("'DATABASE_ENCRYPT' variable must not be null, empty or white space.")
            .Must(value => string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
            .WithMessage("'DATABASE_ENCRYPT' variable must be 'True' or 'False'.");

        RuleFor(property => property.DatabaseCommandTimeout)
            .NotEmpty()
            .WithMessage("'DATABASE_COMMAND_TIMEOUT' variable must not be null, empty or white space.")
            .Must(value => IsValidCommandTimeout(value))
            .WithMessage("'DATABASE_COMMAND_TIMEOUT' variable must be greater than 30 seconds.");

        RuleFor(property => property.AccessTokenSecret)
            .NotEmpty()
            .WithMessage("'ACCESS_TOKEN_SECRET' variable must not be null, empty or white space.")
            .Length(AccessTokenSecretLength)
            .WithMessage($"'ACCESS_TOKEN_SECRET' variable must be exactly {AccessTokenSecretLength} characters long.");

        RuleFor(property => property.AccessTokenExpiresIn)
            .NotEmpty()
            .WithMessage("'ACCESS_TOKEN_EXPIRES_IN' variable must not be null, empty or white space.")
            .MaximumLength(AccessTokenExpiresInLength)
            .WithMessage($"'ACCESS_TOKEN_EXPIRES_IN' must not be longer than {AccessTokenExpiresInLength} characters.")
            .Must(value => IsValidExpiresIn(value))
            .WithMessage("'ACCESS_TOKEN_EXPIRES_IN' must be positive value.");

        RuleFor(property => property.AllowedOrigins)
            .NotEmpty()
            .WithMessage("'ALLOWED_ORIGINS' variable must not be null, empty or white space.");

        RuleFor(property => property.BackgroundJobCron)
            .NotEmpty()
            .WithMessage("'BACKGROUND_JOB_CRON' variable must not be null, empty or white space.");

        RuleFor(property => property.BackgroundJobBatchSize)
            .NotEmpty()
            .WithMessage("'BACKGROUND_JOB_BATCH_SIZE' variable must not be null, empty or white space.")
            .MaximumLength(BackgroundJobBatchSizeLength)
            .WithMessage($"'BACKGROUND_JOB_BATCH_SIZE' must not be longer than {BackgroundJobBatchSizeLength} characters.")
            .Must(value => IsValidBatchSize(value))
            .WithMessage("'BACKGROUND_JOB_BATCH_SIZE' variable must be positive value.");

        RuleFor(property => property.BackgroundJobThreadCount)
            .NotEmpty()
            .WithMessage("'BACKGROUND_JOB_THREAD_COUNT' variable must not be null, empty or white space.")
            .MaximumLength(BackgroundJobThreadCountLength)
            .WithMessage($"'BACKGROUND_JOB_THREAD_COUNT' must not be longer than {BackgroundJobThreadCountLength} characters.")
            .Must(value => IsValidThreadCount(value))
            .WithMessage("'BACKGROUND_JOB_THREAD_COUNT' variable must be greater than 1.");
    }

    private static bool IsValidExpiresIn(string value)
    {
        bool isParsed = long.TryParse(value, out long number);

        if (isParsed)
        {
            return number > 0;
        }

        return false;
    }

    private static bool IsValidBatchSize(string value)
    {
        bool isParsed = int.TryParse(value, out int number);

        if (isParsed)
        {
            return number > 0;
        }

        return false;
    }

    private static bool IsValidCommandTimeout(string value)
    {
        bool isParsed = int.TryParse(value, out int number);

        if (isParsed)
        {
            return number > 30;
        }

        return false;
    }

    private static bool IsValidThreadCount(string value)
    {
        bool isParsed = int.TryParse(value, out int number);

        if (isParsed)
        {
            return number > 1;
        }

        return false;
    }
}
