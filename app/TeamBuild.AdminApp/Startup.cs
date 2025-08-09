using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Marten.Services;
using MudBlazor.Services;
using TeamBuild.AdminApp.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.MudBlazor;
using TeamBuild.Core.Services;
using TeamBuild.Projects.Blazor;
using TeamBuild.Projects.Infrastructure;
using TeamBuild.Projects.MudBlazor;

namespace TeamBuild.AdminApp;

public class Startup
{
    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        ConfigureApplicationServices(builder);
        ConfigureInfrastructureServices(builder);
        ConfigureUiServices(builder);
    }

    public void ConfigureApplicationServices(WebApplicationBuilder builder)
    {
        // Fluent validation
    }

    public void ConfigureInfrastructureServices(WebApplicationBuilder builder)
    {
        var martendbConnectionString =
            builder.Configuration.GetConnectionString("martendb")
            ?? throw new InvalidOperationException(
                "The connection string 'martendb' is not configured."
            );
        builder.Services.AddNpgsqlDataSource(martendbConnectionString);

        builder
            .Services.AddMarten(options =>
            {
                options.UseSystemTextJsonForSerialization();

                options.DatabaseSchemaName = "teambuild";

                options.Events.StreamIdentity = StreamIdentity.AsString;
                options.Events.MetadataConfig.HeadersEnabled = true;
                options.Events.MetadataConfig.CausationIdEnabled = true;
                options.Events.MetadataConfig.CorrelationIdEnabled = true;

                options.OpenTelemetry.TrackEventCounters();
                options.OpenTelemetry.TrackConnections = TrackLevel.Normal;
            })
            .UseNpgsqlDataSource()
            .UseLightweightSessions()
            .ApplyAllDatabaseChangesOnStartup()
            .AddAsyncDaemon(DaemonMode.Solo)
            .AddProjectsMartenServices();

        builder.Services.CritterStackDefaults(options =>
        {
            options.Development.GeneratedCodeMode = TypeLoadMode.Dynamic;
            options.Development.ResourceAutoCreate = AutoCreate.All;

            options.Production.GeneratedCodeMode = TypeLoadMode.Static;
            options.Production.ResourceAutoCreate = AutoCreate.None;
        });

        builder.Services.AddOpenTelemetrySources("Npgsql", "Marten");

        builder.Services.AddProjectsInfrastructureServices();
    }

    public void ConfigureUiServices(WebApplicationBuilder builder)
    {
        builder
            .Services.AddTeamBuildCoreBlazorServices()
            .AddAppInfo(
                new(
                    "Team Build Admin",
                    TeamBuildCoreMudBlazor.UiSelector,
                    [
                        TeamBuildCoreMudBlazor.Assembly,
                        TeamBuildProjectsBlazor.Assembly,
                        TeamBuildProjectsMudBlazor.Assembly,
                    ]
                )
            );

        builder.Services.AddMudServices();

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        builder.Services.AddTeamBuildProjectsBlazorServices();
    }

    public void ConfigureApplication(WebApplication app)
    {
        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        //app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
    }
}
