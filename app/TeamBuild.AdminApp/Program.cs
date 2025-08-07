using MudBlazor.Services;
using TeamBuild.AdminApp.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.MudBlazor;
using TeamBuild.Projects.Blazor;
using TeamBuild.Projects.Infrastructure;
using TeamBuild.Projects.MudBlazor;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

// Infrastructure

builder.Services.AddProjectsInfrastructureServices();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddTeamBuildProjectsBlazorServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
