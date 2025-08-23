using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core.Application.Decorators;

namespace TeamBuild.Core.Application;

public static class ServiceProviderExtensions
{
    public static TService CreateInstance<TService>(
        this IServiceProvider provider,
        params object[] parameters
    )
    {
        return ActivatorUtilities.CreateInstance<TService>(provider, parameters);
    }

    public static IServiceCollection AddDecoratedInfrastructureService<
        TService,
        TImplementation,
        TDecorator,
        TLogging,
        TMetrics,
        TTracing
    >(this IServiceCollection services, ActivitySource activitySource)
        where TLogging : LoggingAspect
        where TMetrics : MetricsAspect
        where TTracing : TracingAspect
        where TService : class
        where TImplementation : TService
        where TDecorator : TService
    {
        return services
            .AddSingleton<TLogging>()
            .AddSingleton<TMetrics>()
            .AddSingleton<TTracing>(provider => provider.CreateInstance<TTracing>(activitySource))
            .AddScoped<TService>(provider =>
                provider.CreateInstance<TDecorator>(provider.CreateInstance<TImplementation>())
            );
    }
}
