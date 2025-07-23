using MudBlazor;
using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.MudBlazor.Components;

public static class TbMudVariantExtensions
{
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
}
