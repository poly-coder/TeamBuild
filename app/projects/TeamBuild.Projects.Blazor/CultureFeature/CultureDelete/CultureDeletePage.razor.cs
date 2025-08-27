using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureDelete;

public sealed partial class CultureDeletePage : IDisposable
{
    [Parameter]
    public string CultureId { get; set; } = "";

    private Domain.CultureFeature.CultureDetails? culture;
    private object JsonContent => new { Title = CultureRoutes.DeleteTitle, Item = culture };

    private TbAsyncOperation<CultureGetByIdQuerySuccess>? load;
    private TbAsyncOperation<CultureDeleteView.FormModel, CultureDeleteCommandSuccess>? delete;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        load = new(LoadRunner)
        {
            OnCompleted = success => culture = success.Culture,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureDeletePage)}.Load",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureResource.DefinitionName,
                operation: TeamBuildCoreDomain.OperationGetByIdName
            ),
        };

        delete = new(DeleteRunner)
        {
            OnCompleted = DeleteCompleted,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureDeletePage)}.Submit",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureResource.DefinitionName,
                operation: TeamBuildCoreDomain.OperationUpdateName
            ),
        };

        load.Execute();
    }

    private async Task<CultureGetByIdQuerySuccess> LoadRunner(CancellationToken cancel)
    {
        return await QueryService.GetById(new(CultureId), cancel);
    }

    private Task<CultureDeleteCommandSuccess> DeleteRunner(
        CultureDeleteView.FormModel form,
        CancellationToken cancel
    )
    {
        return CommandService.Delete(new CultureDeleteCommand(CultureId), cancel);
    }

    private void DeleteCompleted(
        CultureDeleteView.FormModel form,
        CultureDeleteCommandSuccess success
    )
    {
        Nav.NavigateTo(CultureRoutes.List());
    }

    public void Dispose()
    {
        load?.Dispose();
        delete?.Dispose();
    }
}
