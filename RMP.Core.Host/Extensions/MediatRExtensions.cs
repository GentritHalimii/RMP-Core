using System.Reflection;
using RMP.Core.Host.Behaviors;

namespace RMP.Core.Host.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return services;
    }
}