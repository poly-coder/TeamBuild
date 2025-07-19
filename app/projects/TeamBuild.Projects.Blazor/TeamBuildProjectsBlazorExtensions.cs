using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Projects.Blazor;

public static class TeamBuildProjectsBlazorExtensions
{
    public static IServiceCollection AddTeamBuildProjectsBlazorServices(
        this IServiceCollection services
    )
    {
        return services;
    }
}
