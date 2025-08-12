using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudComponentExtensions
{
    public static ButtonType MapToMudButtonType(this TbButtonType source)
    {
        return source switch
        {
            TbButtonType.Button => ButtonType.Button,
            TbButtonType.Reset => ButtonType.Reset,
            TbButtonType.Submit => ButtonType.Submit,
            _ => ButtonType.Button,
        };
    }

    public static Color? MapToMudColor(this TbColor? source)
    {
        return source switch
        {
            TbColor.Default => Color.Default,
            TbColor.Primary => Color.Primary,
            TbColor.Secondary => Color.Secondary,
            TbColor.Tertiary => Color.Tertiary,
            TbColor.Info => Color.Info,
            TbColor.Success => Color.Success,
            TbColor.Warning => Color.Warning,
            TbColor.Error => Color.Error,
            _ => null,
        };
    }

    public static string? MapToMudIcon(this TbIcon? source)
    {
        return source switch
        {
            TbIcon.Add => Icons.Material.Filled.Add,
            TbIcon.Back => Icons.Material.Filled.ArrowBack,
            TbIcon.Delete => Icons.Material.Filled.Delete,
            TbIcon.Cancel => Icons.Material.Filled.Cancel,
            TbIcon.Edit => Icons.Material.Filled.Edit,
            TbIcon.List => Icons.Material.Filled.List,
            TbIcon.Reset => Icons.Material.Filled.Clear,
            TbIcon.Save => Icons.Material.Filled.Save,
            TbIcon.Send => Icons.Material.Filled.Send,
            TbIcon.View => Icons.Material.Filled.RemoveRedEye,
            _ => null,
        };
    }

    public static Size? MapToMudSize(this TbSize? source)
    {
        return source switch
        {
            TbSize.Small => Size.Small,
            TbSize.Medium => Size.Medium,
            TbSize.Large => Size.Large,
            _ => null,
        };
    }

    public static Variant? MapToMudVariant(this TbVariant? source)
    {
        return source switch
        {
            TbVariant.Filled => Variant.Filled,
            TbVariant.Text => Variant.Text,
            TbVariant.Outlined => Variant.Outlined,
            _ => Variant.Filled,
        };
    }

    public static Severity? MapToMudSeverity(this TbSeverity? source)
    {
        return source switch
        {
            TbSeverity.Normal => Severity.Normal,
            TbSeverity.Info => Severity.Info,
            TbSeverity.Success => Severity.Success,
            TbSeverity.Warning => Severity.Warning,
            TbSeverity.Error => Severity.Error,
            _ => null,
        };
    }

    public static BreadcrumbItem MapToBreadcrumbItem(this TbBreadcrumbItem source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new BreadcrumbItem(
            source.Text,
            source.Href,
            source.Disabled,
            source.Icon.MapToMudIcon()
        );
    }

    public static IReadOnlyList<BreadcrumbItem> MapToBreadcrumbItems(
        this IEnumerable<TbBreadcrumbItem> source
    )
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Select(MapToBreadcrumbItem).ToList();
    }
}
