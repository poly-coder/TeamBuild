using Microsoft.AspNetCore.Components;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureList;

public sealed partial class CultureListView
{
    [Parameter]
    public EventCallback<CultureListSearchView.FormModel> OnSearch { get; set; }

    [Parameter]
    public IReadOnlyList<Domain.CultureFeature.CultureDetails> CultureList { get; set; } = [];

    [Parameter]
    public EventCallback<Domain.CultureFeature.CultureDetails> OnViewCulture { get; set; }

    [Parameter]
    public EventCallback<Domain.CultureFeature.CultureDetails> OnEditCulture { get; set; }

    [Parameter]
    public EventCallback<Domain.CultureFeature.CultureDetails> OnDeleteCulture { get; set; }

    private async Task HandleViewCulture(Domain.CultureFeature.CultureDetails? culture)
    {
        if (culture is not null)
            await OnViewCulture.InvokeAsync(culture);
    }

    private async Task HandleEditCulture(Domain.CultureFeature.CultureDetails? culture)
    {
        if (culture is not null)
            await OnEditCulture.InvokeAsync(culture);
    }

    private async Task HandleDeleteCulture(Domain.CultureFeature.CultureDetails? culture)
    {
        if (culture is not null)
            await OnDeleteCulture.InvokeAsync(culture);
    }
}
