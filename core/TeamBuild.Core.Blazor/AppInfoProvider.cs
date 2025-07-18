namespace TeamBuild.Core.Blazor;

public class AppInfoProvider
{
    public string Title { get; set; } = "";

    public string TitleSeparator { get; set; } = " - ";

    public string GetPageTitle(string? pageTitle)
    {
        var titles = new List<string>();

        if (!string.IsNullOrEmpty(pageTitle)) titles.Add(pageTitle);
        if (!string.IsNullOrEmpty(Title)) titles.Add(Title);

        return string.Join(TitleSeparator, titles);
    }
}
