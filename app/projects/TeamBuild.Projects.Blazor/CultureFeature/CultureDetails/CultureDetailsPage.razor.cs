using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureDetails;

public sealed partial class CultureDetailsPage : IDisposable
{
    [Parameter]
    public string CultureId { get; set; } = "";

    private Domain.CultureFeature.CultureDetails? culture;
    private object JsonContent => new { Title = CultureRoutes.DetailsTitle, Item = culture };

    private TbAsyncOperation<CultureGetByIdQuerySuccess>? load;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        load = new(LoadRunner)
        {
            OnCompleted = success => culture = success.Culture,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureDetailsPage)}.Load",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureEntity.Caption,
                operation: TeamBuildCoreDomain.OperationFetchName
            ),
        };

        load.Execute();
    }

    private async Task<CultureGetByIdQuerySuccess> LoadRunner(CancellationToken cancel)
    {
        return await QueryService.GetById(new(CultureId), cancel);
    }

    public void Dispose()
    {
        load?.Dispose();
    }
}
