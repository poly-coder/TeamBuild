using Microsoft.Extensions.DependencyInjection;

namespace TeamBuild.Core.Blazor;

public static class TeamBuildCoreBlazorExtensions
{
    public static IServiceCollection AddTeamBuildCoreBlazorServices(
        this IServiceCollection services
    )
    {
        return services
            .AddSingleton<IMainMenuContainer, MainMenuContainer>()
            .AddScoped<IClipboardService, ClipboardService>();
    }

    public static IServiceCollection AddAppInfo(this IServiceCollection services, AppInfo info)
    {
        return services.AddSingleton(info);
    }
}
