using TeamBuild.Core.Blazor;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Blazor.CultureFeature.CultureNew;

public sealed partial class CultureNewPage : IDisposable
{
    private TbAsyncOperation<CultureNewFormView.FormModel, CultureDefineCommandSuccess>? define;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        define = new(DefineRunner)
        {
            OnCompleted = DefineCompleted,
            OnStageChanged = _ => StateHasChanged(),
            ActivitySource = TeamBuildProjectsBlazor.ActivitySource,
            ActivityName = $"{nameof(CultureNewPage)}.Submit",
            ActivityTags = TeamBuildProjectsBlazor.OperationTags(
                entity: CultureResource.DefinitionName,
                operation: TeamBuildCoreDomain.OperationCreateName
            ),
        };
    }

    private async Task<CultureDefineCommandSuccess> DefineRunner(
        CultureNewFormView.FormModel form,
        CancellationToken cancel
    )
    {
        return await CommandService.Define(form.MapToCommand(), cancel);
    }

    private void DefineCompleted(
        CultureNewFormView.FormModel form,
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
        define?.Dispose();
    }
}
