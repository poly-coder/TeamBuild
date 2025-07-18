using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Core.Blazor;

public static class TeamBuildCoreBlazorExtensions
{
    public static IServiceCollection AddTeamBuildCoreBlazorServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<AppInfoProvider>()
            .AddScoped<IClipboardService, ClipboardService>()
            ;
    }
}
