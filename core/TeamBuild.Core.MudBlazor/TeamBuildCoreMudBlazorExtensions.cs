using Microsoft.Extensions.DependencyInjection;
using TeamBuild.Core.Blazor;

namespace TeamBuild.Core.MudBlazor;

public static class TeamBuildCoreMudBlazorExtensions
{
    public static IServiceCollection AddTeamBuildCoreMudBlazorServices(
        this IServiceCollection services
    )
    {
        return services.AddScoped<IToastService, MudToastService>();
    }
}
