using Microsoft.AspNetCore.Components;
using MudBlazor;
using TeamBuild.Projects.Application.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature;

public class CultureAutocomplete : MudAutocomplete<Domain.CultureFeature.CultureDetails>
{
    [Inject]
    public ICultureQueryService QueryService { get; set; } = null!;

    public CultureAutocomplete()
    {
        Placeholder = "Select culture...";
        HelperText = "Please select a culture from the list.";

        ToStringFunc = c => c.MapToString();

        SearchFunc = async (term, cancel) =>
            (await QueryService.List(new(Filter: new(Search: term)), cancel)).CultureList;

        ItemTemplate = culture =>
            builder =>
            {
                builder.OpenComponent<CultureItemView>(0);
                builder.AddAttribute(1, nameof(CultureItemView.Culture), culture);
                builder.CloseComponent();
            };
    }
}
