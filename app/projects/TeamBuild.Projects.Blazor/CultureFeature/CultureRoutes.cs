using TeamBuild.Core.Blazor;

namespace TeamBuild.Projects.Blazor.CultureFeature;

public static class CultureRoutes
{
    public const string ListRoute = $"{ProjectsRoutes.BasePath}/culture";

    public const string NewRoute = $"{ListRoute}/new";

    public const string DetailsRoute = $"{ListRoute}/details/{{cultureId}}";

    public const string EditRoute = $"{ListRoute}/edit/{{cultureId}}";

    public const string DeleteRoute = $"{ListRoute}/delete/{{cultureId}}";

    public static string List() => ListRoute;

    public static string New() => NewRoute;

    public static string Details(string cultureId) =>
        $"{ListRoute}/details/{Uri.EscapeDataString(cultureId)}";

    public static string Edit(string cultureId) =>
        $"{ListRoute}/edit/{Uri.EscapeDataString(cultureId)}";

    public static string Delete(string cultureId) =>
        $"{ListRoute}/delete/{Uri.EscapeDataString(cultureId)}";

    public static MainMenuItem ListMenuItem =>
        new MainMenuItem("Cultures", ProjectsRoutes.ProjectsCategory, List());
}
