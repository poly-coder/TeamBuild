using MudBlazor.Services;
using TeamBuild.AdminApp.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.MudBlazor;
using TeamBuild.Projects.Blazor;
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

//internal class Categories
//{
//    public static MainMenuCategory Projects = new("Projects", 1000);
//    public static MainMenuCategory Buildings = new("Buildings", 100);
//    public static MainMenuCategory Work = new("Work", 50);
//}

//internal class SampleMainMenuItemProvider1 : IMainMenuItemProvider
//{
//    public IEnumerable<MainMenuItem> GetMenuItems()
//    {
//        yield return new MainMenuItem("Projects", Categories.Projects, "/projects/project");
//        yield return new MainMenuItem(
//            "Buildings",
//            Categories.Buildings,
//            [
//                new MainMenuItem("Zones", "/buildings/zone"),
//                new MainMenuItem("Models", "/buildings/models"),
//            ]
//        );
//        yield return new MainMenuItem("Work Orders", Categories.Work, "/work/work-orders");
//        yield return new MainMenuItem("Tickets", Categories.Work, "/work/tickets");
//        yield return new MainMenuItem("Requests", Categories.Work, "/work/requests");
//    }
//}
