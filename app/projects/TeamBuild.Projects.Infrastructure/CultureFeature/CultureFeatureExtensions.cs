using Marten;
using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core.Application;
using TeamBuild.Core.Application.Decorators;
using TeamBuild.Projects.Application;
using TeamBuild.Projects.Application.CultureFeature;

namespace TeamBuild.Projects.Infrastructure.CultureFeature;

public static class CultureFeatureExtensions
{
    public static IServiceCollection AddCultureInfrastructureServices(
        this IServiceCollection services
    )
    {
        return services
            .ConfigureMarten(options =>
            {
                options.Schema.For<CultureDocument>();
            })
            .AddSingleton<CultureCommandServiceMetricsAspect>()
            .AddScoped<ICultureCommandService>(provider =>
                provider.CreateInstance<CultureCommandServiceDecorator>(
                    provider.CreateInstance<CultureCommandMartenService>(),
                    new TracingAspect(TeamBuildProjectsApplication.ActivitySource)
                )
            );
    }
}
