using Microsoft.Extensions.DependencyInjection;

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
}
