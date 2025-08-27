using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain.CultureFeature;

public static class AvailableCultureResource
{
    public const string DefinitionName = "available-culture";

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
            Operations = [AvailableCultureListQuery.Definition],
        };
    }
}
