using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TeamBuild.Core;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Domain;

public static class TeamBuildProjectsDomain
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static readonly string Name = Assembly.GetAssemblyName();
    public static readonly string Version = Assembly.GetAssemblyVersion();

    public static readonly ActivitySource ActivitySource = new(Name, Version);

    public static readonly Meter Meter = new(Name, Version);

    public static IEnumerable<JsonSerializerContext> GetJsonContexts(JsonSerializerOptions options)
    {
        yield return new CultureJsonContext(options);
    }
}
