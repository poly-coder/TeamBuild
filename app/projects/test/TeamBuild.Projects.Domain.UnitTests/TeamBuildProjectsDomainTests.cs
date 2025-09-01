using TeamBuild.Core.Testing;

namespace TeamBuild.Projects.Domain.UnitTests;

[UnitTest]
[ProjectsTest]
[DomainLayerTest]
public class TeamBuildProjectsDomainTests
{
    [Fact]
    public void DefinitionTest()
    {
        typeof(TeamBuildProjectsDomain).ShouldBeProperlyDefined(
            TeamBuildProjectsDomain.Assembly,
            TeamBuildProjectsDomain.Name,
            TeamBuildProjectsDomain.Version,
            TeamBuildProjectsDomain.ActivitySource,
            TeamBuildProjectsDomain.Meter
        );
    }
}
