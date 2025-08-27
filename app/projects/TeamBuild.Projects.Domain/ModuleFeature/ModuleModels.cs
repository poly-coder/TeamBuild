namespace TeamBuild.Projects.Domain.ModuleFeature;

public abstract class ModuleElement
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string FullName { get; init; }
}

public class ModuleDefinition : ModuleElement
{
    public required IReadOnlyCollection<ResourceDefinition> Resources { get; init; }
}

public class ResourceDefinition : ModuleElement
{
    public required IReadOnlyCollection<ResourceDefinition> Resources { get; init; }
    public required IReadOnlyCollection<OperationDefinition> Operations { get; init; }
}

public class OperationDefinition : ModuleElement { }
