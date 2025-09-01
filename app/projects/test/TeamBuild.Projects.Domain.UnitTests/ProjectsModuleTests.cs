using TeamBuild.Core.Testing;

namespace TeamBuild.Projects.Domain.UnitTests;

[UnitTest]
[ProjectsTest]
[DomainLayerTest]
public class ProjectsModuleTests
{
    [Fact]
    public async Task DefinitionTest()
    {
        var definition = ProjectsModule.Definition;

        await Verify(new { definition }).UseSnapshotsDirectory();
    }
}
