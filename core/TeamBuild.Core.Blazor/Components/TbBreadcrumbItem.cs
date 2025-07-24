namespace TeamBuild.Core.Blazor.Components;

public class TbBreadcrumbItem
{
    public required string Text { get; init; }

    public string? Href { get; init; }

    public bool Disabled { get; init; }

    public TbIcon? Icon { get; init; }

    public static TbBreadcrumbItem Current(string text, TbIcon? icon = null) =>
        new TbBreadcrumbItem()
        {
            Text = text,
            Icon = icon,
            Disabled = true,
        };

    public static TbBreadcrumbItem Create(string text, string href, TbIcon? icon = null) =>
        new TbBreadcrumbItem()
        {
            Text = text,
            Href = href,
            Icon = icon,
        };
}
