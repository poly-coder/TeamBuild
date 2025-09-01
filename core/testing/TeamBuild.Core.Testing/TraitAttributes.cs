using Xunit.Sdk;

namespace TeamBuild.Core.Testing;

// Categories

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class UnitTestAttribute : Attribute, ITraitAttribute
{
    public UnitTestAttribute(string name = "Category", string value = "UnitTest") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
{
    public IntegrationTestAttribute(string name = "Category", string value = "IntegrationTest") { }
}

// Layers

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class PreambleLayerTestAttribute : Attribute, ITraitAttribute
{
    public PreambleLayerTestAttribute(string name = "Layer", string value = "Preamble") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class DomainLayerTestAttribute : Attribute, ITraitAttribute
{
    public DomainLayerTestAttribute(string name = "Layer", string value = "Domain") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class ApplicationLayerTestAttribute : Attribute, ITraitAttribute
{
    public ApplicationLayerTestAttribute(string name = "Layer", string value = "Application") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class InfrastructureLayerTestAttribute : Attribute, ITraitAttribute
{
    public InfrastructureLayerTestAttribute(string name = "Layer", string value = "Infrastructure")
    { }
}

// Projects

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class CoreProjectTestAttribute : Attribute, ITraitAttribute
{
    public CoreProjectTestAttribute(string name = "Project", string value = "Core") { }
}
