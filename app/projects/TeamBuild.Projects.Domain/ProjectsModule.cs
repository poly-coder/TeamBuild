using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.CultureFeature;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain;

public static class ProjectsModule
{
    public const string DefinitionName = "projects";
    public static readonly DefinitionKey DefinitionKey = DefinitionKey.Module(DefinitionName);

    private static readonly Lazy<ModuleDefinition> LazyDefinition = new(CreateDefinition);
    public static ModuleDefinition Definition => LazyDefinition.Value;

    private static ModuleDefinition CreateDefinition()
    {
        return new ModuleDefinition
        {
            Name = DefinitionName,
            Id = DefinitionKey.Id,
            FullName = DefinitionKey.FullName,
            Resources =
            [
                AvailableCultureResource.Definition,
                CultureResource.Definition,
                ModuleResource.Definition,
            ],
        };
    }
}
