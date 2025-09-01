using Xunit.Sdk;

namespace TeamBuild.Projects.Infrastructure.UnitTests;

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class ProjectsTestAttribute : Attribute, ITraitAttribute
{
    public ProjectsTestAttribute(string name = "Project", string value = "Projects") { }
}
