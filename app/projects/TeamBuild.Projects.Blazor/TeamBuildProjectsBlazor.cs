using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using TeamBuild.Core;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain;

namespace TeamBuild.Projects.Blazor;

public static class TeamBuildProjectsBlazor
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static readonly string Name = Assembly.GetAssemblyName();
    public static readonly string Version = Assembly.GetAssemblyVersion();

    public static readonly ActivitySource ActivitySource = new(Name, Version);

    public static readonly Meter Meter = new(Name, Version);

    public static IEnumerable<KeyValuePair<string, object?>> OperationTags(
        string? entity = null,
        string? operation = null
    )
    {
        return TeamBuildCoreDomain.OperationTags(
            project: ProjectsModule.DefinitionName,
            layer: TeamBuildCoreDomain.LayerUiName,
            entity: entity,
            operation: operation
        );
    }
}
