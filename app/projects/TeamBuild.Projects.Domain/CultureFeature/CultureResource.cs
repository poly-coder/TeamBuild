using System.Diagnostics.Metrics;
using TeamBuild.Core.Domain;
using TeamBuild.Projects.Domain.ModuleFeature;

namespace TeamBuild.Projects.Domain.CultureFeature;

public static class CultureResource
{
    public const string DefinitionName = "culture";

    public static readonly DefinitionKey DefinitionKey = ProjectsModule.DefinitionKey.Resource(
        DefinitionName
    );

    public static readonly UpDownCounter<long> Counter =
        TeamBuildProjectsDomain.Meter.CreateUpDownCounter<long>(DefinitionKey.FullName);

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
            Operations =
            [
                CultureDefineCommand.Definition,
                CultureDeleteCommand.Definition,
                CultureListQuery.Definition,
                CultureGetByIdQuery.Definition,
                CultureGetByIdsQuery.Definition,
            ],
        };
    }
}
