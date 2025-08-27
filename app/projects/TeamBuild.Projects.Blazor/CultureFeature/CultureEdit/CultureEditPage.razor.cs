using Microsoft.AspNetCore.Components;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureEdit;

public sealed partial class CultureEditPage : IDisposable
{
    [Parameter]
    public string CultureId { get; set; } = "";

    private Domain.CultureFeature.CultureDetails? culture;
    private object JsonContent => new { Title = CultureRoutes.EditTitle, Item = culture };

    private TbAsyncOperation<CultureGetByIdQuerySuccess>? load;
    private TbAsyncOperation<CultureEditView.FormModel, CultureDefineCommandSuccess>? define;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        load = new(LoadRunner)
        {
            OnCompleted = success => culture = success.Culture,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureEditPage)}.Load",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureResource.DefinitionName,
                operation: TeamBuildCoreDomain.OperationGetByIdName
            ),
        };

        define = new(DefineRunner)
        {
            OnCompleted = DefineCompleted,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureEditPage)}.Submit",
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

    private Task<CultureDefineCommandSuccess> DefineRunner(
        CultureEditView.FormModel form,
        CancellationToken cancel
    )
    {
        return CommandService.Define(form.MapToCommand(), cancel);
    }

    private void DefineCompleted(
        CultureEditView.FormModel form,
        CultureDefineCommandSuccess success
    )
    {
        var route = form.GoToDetails
            ? CultureRoutes.Details(success.Culture.CultureCode)
            : CultureRoutes.List();

        Nav.NavigateTo(route);
    }

    public void Dispose()
    {
        load?.Dispose();
        define?.Dispose();
    }
}
