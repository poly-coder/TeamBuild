using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.ModuleFeature;

public static class ModuleResource
{
    public const string DefinitionName = "module";

    public static readonly DefinitionKey DefinitionKey = ProjectsModule.DefinitionKey.Resource(
        DefinitionName
    );

    private static readonly Lazy<ResourceDefinition> LazyDefinition = new(CreateDefinition);
    public static ResourceDefinition Definition => LazyDefinition.Value;

    private static ResourceDefinition CreateDefinition()
    {
        return new ResourceDefinition
        {
            Name = DefinitionName,
            Id = DefinitionKey.Id,
            FullName = DefinitionKey.FullName,
            Resources = [],
            Operations = [],
        };
    }
}
