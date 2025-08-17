using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace TeamBuild.Core.Blazor;

public static class TeamBuildCoreBlazor
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static readonly string Name = Assembly.GetAssemblyName();
    public static readonly string Version = Assembly.GetAssemblyVersion();

    public static readonly ActivitySource ActivitySource = new(Name, Version);

    public static readonly Meter Meter = new(Name, Version);
}
