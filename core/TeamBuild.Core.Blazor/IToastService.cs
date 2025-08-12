using TeamBuild.Core.Blazor.Components;

namespace TeamBuild.Core.Blazor;

public interface IToastService
{
    void ShowToast(string message, TbSeverity? severity = null);
}
