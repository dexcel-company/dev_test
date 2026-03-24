using Asp.Versioning;
using CelloPark.Api.Common.ExceptionHandlers;
using CelloPark.Api.Common.Versioning;
using CelloPark.Api.Common.Versioning.Constants;
using CelloPark.Api.Features.Customers.Validators;
using CelloPark.Api.Features.Items.Validators;
using CelloPark.Application.Features.Customers.Entities.CustomerCouponUsages.Dtos;
using CelloPark.Application.Features.Items.Dtos;
using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CelloPark.Api.Common.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddProblemDetails()
            .AddExceptionHandlers()
            .AddEndpointsApiExplorer()
            .AddVersioning()
            .AddSwagger()
            .AddEndpointValidators();

        return services;
    }

    private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<BadHttpRequestExceptionHandler>();
        services.AddExceptionHandler<InternalExceptionHandler>();

        return services;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(VersioningSettings.DefaultMajorApiVersion, VersioningSettings.DefaultMinorApiVersion);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddEndpointValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CustomerCouponUsageCreateDto>, CreateCouponUsageValidator>();
        services.AddSingleton<IValidator<ItemCreateDto>, CreateItemValidator>();
        services.AddSingleton<IValidator<ItemUpdateDto>, UpdateItemValidator>();

        return services;
    }
}
