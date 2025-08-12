using Marten;
using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core.Services;
using TeamBuild.Projects.Application;
using TeamBuild.Projects.Infrastructure.CultureFeature;

namespace TeamBuild.Projects.Infrastructure;

public static class TeamBuildProjectsInfrastructureExtensions
{
    public static IServiceCollection AddProjectsInfrastructureServices(
        this IServiceCollection services
    )
    {
        return services
            .AddCultureInfrastructureServices()
            .AddOpenTelemetrySources(
                TeamBuildProjectsApplication.Name,
                TeamBuildProjectsInfrastructure.Name
            );
    }

    public static MartenServiceCollectionExtensions.MartenConfigurationExpression AddProjectsMartenServices(
        this MartenServiceCollectionExtensions.MartenConfigurationExpression services
    )
    {
        return services;
    }
}
