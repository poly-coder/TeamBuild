using TeamBuild.Core.Application.Testing;
using TeamBuild.Core.Domain;
using TeamBuild.Core.Testing;
using TeamBuild.Projects.Domain;

namespace TeamBuild.Projects.Application.UnitTests;

[UnitTest]
[ProjectsTest]
[ApplicationLayerTest]
public class TeamBuildProjectsApplicationTests
{
    [Fact]
    public void DefinitionTest()
    {
        typeof(TeamBuildProjectsApplication).ShouldBeProperlyDefined(
            TeamBuildProjectsApplication.Assembly,
            TeamBuildProjectsApplication.Name,
            TeamBuildProjectsApplication.Version,
            TeamBuildProjectsApplication.ActivitySource,
            TeamBuildProjectsApplication.Meter
        );
    }

    [Fact]
    public void OperationTagsTest()
    {
        TeamBuildProjectsApplication
            .OperationTags("entity-name", "operation-name")
            .ShouldHaveOperationTags(
                ProjectsModule.DefinitionName,
                TeamBuildCoreDomain.LayerApplicationName,
                "entity-name",
                "operation-name"
            );
    }
}
