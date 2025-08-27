using TeamBuild.Core.Blazor;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureList;

public sealed partial class CultureListPage : IDisposable
{
    private readonly List<Domain.CultureFeature.CultureDetails> cultureList = [];
    private object JsonContent => new { Title = CultureRoutes.ListTitle, Items = cultureList };
    private CultureListSearchView.FormModel lastSearch = new();

    private TbAsyncOperation<CultureListSearchView.FormModel, CultureListQuerySuccess>? search;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        search = new(SearchRunner)
        {
            OnCompleted = SearchCompleted,
            OnRunning = model => lastSearch = model,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureListPage)}.Search",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureResource.DefinitionName,
                operation: TeamBuildCoreDomain.OperationListName
            ),
        };
        search.Execute(lastSearch);
    }

    private async Task<CultureListQuerySuccess> SearchRunner(
        CultureListSearchView.FormModel form,
        CancellationToken cancel
    )
    {
        return await QueryService.List(form.MapToQuery(), cancel);
    }

    private void SearchCompleted(CultureListSearchView.FormModel _, CultureListQuerySuccess success)
    {
        cultureList.Clear();
        cultureList.AddRange(success.CultureList);
    }

    private void HandleOnViewCulture(Domain.CultureFeature.CultureDetails culture)
    {
        Nav.NavigateTo(CultureRoutes.Details(culture.CultureCode));
    }

    private void HandleOnEditCulture(Domain.CultureFeature.CultureDetails culture)
    {
        Nav.NavigateTo(CultureRoutes.Edit(culture.CultureCode));
    }

    private void HandleOnDeleteCulture(Domain.CultureFeature.CultureDetails culture)
    {
        Nav.NavigateTo(CultureRoutes.Delete(culture.CultureCode));
    }

    public void Dispose()
    {
        search?.Dispose();
    }
}
