using Microsoft.AspNetCore.Components;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureNew;

public sealed partial class CultureNewView
{
    private CultureNewFormView.FormModel initialForm = new();

    [Parameter]
    public EventCallback<CultureNewFormView.FormModel> OnCreate { get; set; }

    [Parameter]
    public bool IsCreating { get; set; }

    [Parameter]
    public Exception? CreateException { get; set; }

    private void HandleAvailableCultureChanged(Domain.CultureFeature.CultureDetails culture)
    {
        initialForm = culture.MapToNewForm();
    }
}
