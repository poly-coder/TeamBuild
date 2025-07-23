using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudColorExtensions
{
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
}
