using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace TeamBuild.Core.Blazor;

public abstract class TbCustomView : ComponentBase
{
    [Inject]
    public AppInfo AppInfo { get; set; } = default!;

    protected bool TryRender(RenderTreeBuilder builder)
    {
        Type viewBaseType = GetType();

        if (AppInfo.TryGetCustomViewType(viewBaseType, out var viewType))
        {
            var sequence = 0;
            builder.OpenComponent(sequence++, viewType);

            foreach (
                var property in viewBaseType.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                )
            )
            {
                if (
                    property is { CanRead: true, CanWrite: true }
                    && property.GetCustomAttribute<ParameterAttribute>() is { }
                )
                {
                    var value = property.GetValue(this);
                    if (value != null)
                    {
                        builder.AddAttribute(sequence++, property.Name, value);
                    }
                }
            }

            builder.CloseComponent();
            return true;
        }

        return false;
    }

    protected virtual void RenderDefault(RenderTreeBuilder builder)
    {
        Type viewBaseType = GetType();

        builder.AddMarkupContent(
            0,
            $$"""<p style="color:red;">Cannot render {{AppInfo.UiSelector}} view for {{viewBaseType.FullName}}</p>"""
        );
    }

    protected void Render(RenderTreeBuilder builder)
    {
        if (!TryRender(builder))
        {
            RenderDefault(builder);
        }
    }
}
