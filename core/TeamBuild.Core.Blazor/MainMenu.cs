namespace TeamBuild.Core.Blazor;

public interface IMainMenuContainer
{
    IEnumerable<(MainMenuCategory, IEnumerable<MainMenuItem>)> GetMenuItems();
}

public class MainMenuContainer : IMainMenuContainer
{
    private static readonly MainMenuCategory DefaultCategory = new("Other", int.MinValue);

    private readonly IEnumerable<IMainMenuItemProvider> providers;

    public MainMenuContainer(IEnumerable<IMainMenuItemProvider> providers)
    {
        this.providers = providers;
        ArgumentNullException.ThrowIfNull(providers);
    }

    public IEnumerable<(MainMenuCategory, IEnumerable<MainMenuItem>)> GetMenuItems() =>
        providers
            .SelectMany(p => p.GetMenuItems())
            .GroupBy(i => i.Category ?? DefaultCategory)
            .OrderByDescending(g => g.Key.Priority)
            .ThenBy(g => g.Key.Label())
            .Select(g =>
                (g.Key, g.OrderByDescending(i => i.Priority).ThenBy(i => i.Label()).AsEnumerable())
            );
}

public interface IMainMenuItemProvider
{
    IEnumerable<MainMenuItem> GetMenuItems();
}

public record MainMenuCategory(Func<string> Label, int Priority = 0)
{
    public MainMenuCategory(string label, int priority = 0)
        : this(() => label, priority)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
    }
}

public record MainMenuItem(
    Func<string> Label,
    MainMenuCategory? Category,
    string? Route,
    IReadOnlyList<MainMenuItem>? Children,
    int Priority = 0
)
{
    public MainMenuItem(
        Func<string> label,
        MainMenuCategory category,
        string route,
        int priority = 0
    )
        : this(label, category, route, null, priority)
    {
        ArgumentNullException.ThrowIfNull(category);
        ArgumentNullException.ThrowIfNull(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
    }

    public MainMenuItem(string label, MainMenuCategory category, string route, int priority = 0)
        : this(() => label, category, route, null, priority)
    {
        ArgumentNullException.ThrowIfNull(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
    }

    public MainMenuItem(Func<string> label, string route, int priority = 0)
        : this(label, null, route, null, priority)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
    }

    public MainMenuItem(string label, string route, int priority = 0)
        : this(() => label, null, route, null, priority)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
    }

    public MainMenuItem(
        Func<string> label,
        MainMenuCategory category,
        IReadOnlyList<MainMenuItem> children,
        int priority = 0
    )
        : this(label, category, null, children, priority)
    {
        ArgumentNullException.ThrowIfNull(category);
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(children);
        ArgumentOutOfRangeException.ThrowIfZero(children.Count);
    }

    public MainMenuItem(
        string label,
        MainMenuCategory category,
        IReadOnlyList<MainMenuItem> children,
        int priority = 0
    )
        : this(() => label, category, null, children, priority)
    {
        ArgumentNullException.ThrowIfNull(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentNullException.ThrowIfNull(children);
        ArgumentOutOfRangeException.ThrowIfZero(children.Count);
    }

    public MainMenuItem(Func<string> label, IReadOnlyList<MainMenuItem> children, int priority = 0)
        : this(label, null, null, children, priority)
    {
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(children);
        ArgumentOutOfRangeException.ThrowIfZero(children.Count);
    }

    public MainMenuItem(string label, IReadOnlyList<MainMenuItem> children, int priority = 0)
        : this(() => label, null, null, children, priority)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentNullException.ThrowIfNull(children);
        ArgumentOutOfRangeException.ThrowIfZero(children.Count);
    }
}
