using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TeamBuild.Core.Blazor;

public class AppInfo
{
    private readonly FrozenDictionary<(Type, string), Type> customViewTypes;

    public AppInfo(
        string title,
        string uiSelector,
        IEnumerable<Assembly> uiAssemblies,
        string titleSeparator = " - "
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(uiSelector);
        ArgumentNullException.ThrowIfNull(uiAssemblies);
        ArgumentNullException.ThrowIfNull(titleSeparator);

        Title = title;
        UiSelector = uiSelector;
        UiAssemblies = uiAssemblies.ToList().AsReadOnly();
        TitleSeparator = titleSeparator;

        customViewTypes = FindCustomViewTypes();
    }

    public string Title { get; }

    public string TitleSeparator { get; }

    public string UiSelector { get; }

    public IReadOnlyCollection<Assembly> UiAssemblies { get; }

    public string GetPageTitle(string? pageTitle)
    {
        var titles = new List<string>();

        if (!string.IsNullOrEmpty(pageTitle))
            titles.Add(pageTitle);
        if (!string.IsNullOrEmpty(Title))
            titles.Add(Title);

        return string.Join(TitleSeparator, titles);
    }

    public bool TryGetCustomViewType(Type viewBaseType, [NotNullWhen(true)] out Type? viewType)
    {
        return customViewTypes.TryGetValue((viewBaseType, UiSelector), out viewType);
    }

    private FrozenDictionary<(Type, string), Type> FindCustomViewTypes()
    {
        var types = new Dictionary<(Type, string), Type>();

        foreach (var assembly in UiAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (
                    !type.IsAbstract
                    && type.BaseType != typeof(TbCustomView)
                    && type.IsSubclassOf(typeof(TbCustomView))
                    && type.GetCustomAttribute<CustomViewAttribute>() is { } attr
                )
                {
                    var key = (type.BaseType!, attr.Selector);
                    if (!types.TryAdd(key, type))
                    {
                        throw new InvalidOperationException(
                            $"Duplicate custom view type for '{type.BaseType!.FullName}' found for selector '{attr.Selector}' in '{type.FullName}' and '{types[key]!.FullName}'."
                        );
                    }
                }
            }
        }

        return types.ToFrozenDictionary();
    }
}
