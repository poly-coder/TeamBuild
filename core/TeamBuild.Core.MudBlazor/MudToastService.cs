using MudBlazor;
using TeamBuild.Core.Blazor;
using TeamBuild.Core.Blazor.Components;
using TeamBuild.Core.MudBlazor.Components;

namespace TeamBuild.Core.MudBlazor;

public class MudToastService(ISnackbar snackbar) : IToastService
{
    public void ShowToast(string message, TbSeverity? severity = null)
    {
        snackbar.Add(message, severity.MapToMudSeverity() ?? Severity.Normal);
    }
}
