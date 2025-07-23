using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor;

public static class TeamBuildProjectsBlazorExtensions
{
    public static IServiceCollection AddTeamBuildProjectsBlazorServices(
        this IServiceCollection services
    )
    {
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IMainMenuItemProvider, ProjectsMainMenuItemsProvider>()
        );

        return services;
    }
}
