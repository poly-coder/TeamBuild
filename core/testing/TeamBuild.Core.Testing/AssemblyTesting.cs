using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using Shouldly;

namespace TeamBuild.Core.Testing;

public static class AssemblyTesting
{
    public static void ShouldBeProperlyDefined(
        this Type assemblyDefinitionType,
        Assembly assembly,
        string name,
        string version,
        ActivitySource activitySource,
        Meter meter
    )
    {
        assembly.ShouldSatisfyAllConditions(
            () => assembly.ShouldBe(assemblyDefinitionType.Assembly),
            () => name.ShouldBe(assembly.GetAssemblyName()),
            () => version.ShouldBe(assembly.GetAssemblyVersion()),
            () => activitySource.Name.ShouldBe(name),
            () => activitySource.Version.ShouldBe(version),
            () => meter.Name.ShouldBe(name),
            () => meter.Version.ShouldBe(version)
        );
    }
}
