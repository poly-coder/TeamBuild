using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudIconExtensions
{
    public static string? MapToMudIcon(this TbIcon? source)
    {
        return source switch
        {
            TbIcon.Add => Icons.Material.Filled.Add,
            TbIcon.Back => Icons.Material.Filled.ArrowBack,
            TbIcon.Delete => Icons.Material.Filled.Delete,
            TbIcon.Edit => Icons.Material.Filled.Edit,
            TbIcon.Save => Icons.Material.Filled.Save,
            TbIcon.Send => Icons.Material.Filled.Send,
            TbIcon.View => Icons.Material.Filled.RemoveRedEye,
            _ => null,
        };
    }
}
