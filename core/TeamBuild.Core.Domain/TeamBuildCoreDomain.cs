using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace TeamBuild.Core.Domain;

public class TeamBuildCoreDomain
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static readonly string Name = Assembly.GetAssemblyName();
    public static readonly string Version = Assembly.GetAssemblyVersion();

    public static readonly ActivitySource ActivitySource = new(Name, Version);

    public static readonly Meter Meter = new(Name, Version);

    public const string AppSystemTagKey = "app.system";
    public const string AppProjectTagKey = "app.project";
    public const string AppEntityTagKey = "app.entity";
    public const string AppOperationTagKey = "app.operation";
    public const string AppLayerTagKey = "app.layer";

    public const string TeamBuildSystemName = "TeamBuild";

    public const string OperationListName = "list";
    public const string OperationFetchName = "fetch";
    public const string OperationCreateName = "create";
    public const string OperationUpdateName = "update";
    public const string OperationDeleteName = "delete";

    public const string LayerApplicationName = "app";
    public const string LayerInfrastructureName = "infra";
    public const string LayerUiName = "ui";

    public static IEnumerable<KeyValuePair<string, object?>> OperationTags(
        string? project = null,
        string? layer = null,
        string? entity = null,
        string? operation = null
    )
    {
        yield return KeyValuePair.Create(AppSystemTagKey, (object?)TeamBuildSystemName);
        if (!string.IsNullOrEmpty(project))
            yield return KeyValuePair.Create(AppProjectTagKey, (object?)project);
        if (!string.IsNullOrEmpty(layer))
            yield return KeyValuePair.Create(AppLayerTagKey, (object?)layer);
        if (!string.IsNullOrEmpty(entity))
            yield return KeyValuePair.Create(AppEntityTagKey, (object?)entity);
        if (!string.IsNullOrEmpty(operation))
            yield return KeyValuePair.Create(AppOperationTagKey, (object?)operation);
    }
}
