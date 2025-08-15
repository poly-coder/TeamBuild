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

    // Titles

    public const string ListTitle = "Culture List";
    public const string NewTitle = "Add Culture";
    public const string DetailsTitle = "Culture Details";
    public const string EditTitle = "Edit Culture";
    public const string DeleteTitle = "Delete Culture";

    // Breadcrumbs

    public static readonly IReadOnlyList<BreadcrumbItem> ListBreadcrumbs =
    [
        TbMudBreadcrumb.CurrentList(ListTitle),
    ];

    public static readonly IReadOnlyList<BreadcrumbItem> NewBreadcrumbs =
    [
        TbMudBreadcrumb.CreateList(ListTitle, List()),
        TbMudBreadcrumb.CurrentAdd(NewTitle),
    ];

    public static IReadOnlyList<BreadcrumbItem> DetailsBreadcrumbs =>
        [
            TbMudBreadcrumb.CreateList(ListTitle, List()),
            TbMudBreadcrumb.CurrentDetails(DetailsTitle),
        ];

    public static IReadOnlyList<BreadcrumbItem> DeleteBreadcrumbs(string cultureId) =>
        [
            TbMudBreadcrumb.CreateList(ListTitle, List()),
            TbMudBreadcrumb.CreateDetails(DetailsTitle, Details(cultureId)),
            TbMudBreadcrumb.CurrentDelete(DeleteTitle),
        ];

    public static IReadOnlyList<BreadcrumbItem> EditBreadcrumbs(string cultureId) =>
        [
            TbMudBreadcrumb.CreateList(ListTitle, List()),
            TbMudBreadcrumb.CreateDetails(DetailsTitle, Details(cultureId)),
            TbMudBreadcrumb.CurrentEdit(EditTitle),
        ];
}
