using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Contexts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CelloPark.Application.Common.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly currentAssembly = typeof(IManagementContext).Assembly;

        services
            .AddHandlers(
                currentAssembly,
                typeof(SingletonHandlerAttribute),
                (serviceType, implementationType) => services.AddSingleton(serviceType, implementationType))
            .AddHandlers(
                currentAssembly,
                typeof(ScopedHandlerAttribute),
                (serviceType, implementationType) => services.AddScoped(serviceType, implementationType));

        return services;
    }

    private static IServiceCollection AddHandlers(
        this IServiceCollection services,
        Assembly assembly,
        Type attributeType,
        Func<Type, Type, object> registerService)
    {
        IEnumerable<Type> serviceTypes = assembly
            .GetTypes()
            .Where(type => type.IsInterface && type.GetCustomAttributes(attributeType, false).Length != 0);

        foreach (Type serviceType in serviceTypes)
        {
            Type? implementationType = assembly
                .GetTypes()
                .FirstOrDefault(type => serviceType.IsAssignableFrom(type) && type.IsClass);

            if (implementationType is not null)
            {
                registerService(serviceType, implementationType);
            }
        }

        return services;
    }
}
