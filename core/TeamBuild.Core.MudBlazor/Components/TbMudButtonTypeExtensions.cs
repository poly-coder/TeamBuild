using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudButtonTypeExtensions
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
}
