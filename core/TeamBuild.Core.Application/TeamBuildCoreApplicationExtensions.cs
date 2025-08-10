using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core.Application.Decorators;

namespace TeamBuild.Core.Application;

public static class TeamBuildCoreApplicationExtensions
{
    public static IServiceCollection AddCoreApplicationServices(this IServiceCollection services)
    {
        return services.AddSingleton<LoggingAspect>().AddScoped<FluentValidationAspect>();
    }
}
