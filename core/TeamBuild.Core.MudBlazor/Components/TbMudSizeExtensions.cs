using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudSizeExtensions
{
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
}
