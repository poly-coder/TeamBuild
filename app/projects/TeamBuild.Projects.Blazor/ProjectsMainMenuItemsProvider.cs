using TeamBuild.Core.Blazor;
using TeamBuild.Projects.Blazor.CultureFeature;

namespace TeamBuild.Projects.Blazor;

public class ProjectsMainMenuItemsProvider : IMainMenuItemProvider
{
    public IEnumerable<MainMenuItem> GetMenuItems()
    {
        yield return CultureRoutes.ListMenuItem;
    }
}
