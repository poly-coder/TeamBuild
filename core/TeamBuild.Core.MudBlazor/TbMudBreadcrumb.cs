using MudBlazor;

namespace TeamBuild.Core.MudBlazor;

public static class TbMudBreadcrumb
{
    public static BreadcrumbItem Current(string text, string? icon = null) =>
        new(text, null, true, icon);

    public static BreadcrumbItem Create(string text, string route, string? icon = null) =>
        new(text, route, false, icon);

    public static BreadcrumbItem CurrentList(string text) => Current(text, TbMudIcons.List);

    public static BreadcrumbItem CreateList(string text, string route) =>
        Create(text, route, TbMudIcons.List);

    public static BreadcrumbItem CurrentAdd(string text) => Current(text, TbMudIcons.Add);

    public static BreadcrumbItem CreateAdd(string text, string route) =>
        Create(text, route, TbMudIcons.Add);

    public static BreadcrumbItem CurrentDetails(string text) => Current(text, TbMudIcons.Details);

    public static BreadcrumbItem CreateDetails(string text, string route) =>
        Create(text, route, TbMudIcons.Details);

    public static BreadcrumbItem CurrentDelete(string text) => Current(text, TbMudIcons.Delete);

    public static BreadcrumbItem CreateDelete(string text, string route) =>
        Create(text, route, TbMudIcons.Delete);

    public static BreadcrumbItem CurrentEdit(string text) => Current(text, TbMudIcons.Edit);

    public static BreadcrumbItem CreateEdit(string text, string route) =>
        Create(text, route, TbMudIcons.Edit);
}
