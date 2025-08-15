using MudBlazor;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.MudBlazor;

namespace TeamBuild.Projects.Blazor.CultureFeature;

public static class CultureRoutes
{
    // Page Routes

    public const string ListRoute = $"{ProjectsRoutes.BasePath}/culture";

    public const string NewRoute = $"{ListRoute}/new";

    public const string DetailsRoute = $$"""{{ListRoute}}/details/{cultureId}""";

    public const string EditRoute = $$"""{{ListRoute}}/edit/{cultureId}""";

    public const string DeleteRoute = $$"""{{ListRoute}}/delete/{cultureId}""";

    // Navigation

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

    // Breadcrumbs

    public static readonly IReadOnlyList<BreadcrumbItem> ListBreadcrumbs =
    [
        TbMudBreadcrumb.CurrentList("Culture List"),
    ];

    public static readonly IReadOnlyList<BreadcrumbItem> NewBreadcrumbs =
    [
        TbMudBreadcrumb.CreateList("Culture List", List()),
        TbMudBreadcrumb.CurrentAdd("Add Culture"),
    ];

    public static IReadOnlyList<BreadcrumbItem> DetailsBreadcrumbs =>
        [
            TbMudBreadcrumb.CreateList("Cultures", List()),
            TbMudBreadcrumb.CurrentDetails("Culture Details"),
        ];

    public static IReadOnlyList<BreadcrumbItem> DeleteBreadcrumbs(string cultureId) =>
        [
            TbMudBreadcrumb.CreateList("Cultures", List()),
            TbMudBreadcrumb.CreateDetails("Culture Details", Details(cultureId)),
            TbMudBreadcrumb.CurrentDelete("Delete Culture"),
        ];

    public static IReadOnlyList<BreadcrumbItem> EditBreadcrumbs(string cultureId) =>
        [
            TbMudBreadcrumb.CreateList("Cultures", List()),
            TbMudBreadcrumb.CreateDetails("Culture Details", Details(cultureId)),
            TbMudBreadcrumb.CurrentEdit("Edit Culture"),
        ];
}
