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
        TServiceAspect
    >(this IServiceCollection services)
        where TServiceAspect : StandardServiceAspect
        where TService : class
        where TImplementation : TService
        where TDecorator : TService
    {
        return services
            .AddSingleton<TServiceAspect>()
            .AddScoped<TService>(provider =>
                provider.CreateInstance<TDecorator>(provider.CreateInstance<TImplementation>())
            );
    }
}
