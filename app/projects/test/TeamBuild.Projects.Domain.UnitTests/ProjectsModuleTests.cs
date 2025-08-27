using TeamBuild.Core.Testing;

namespace TeamBuild.Projects.Domain.UnitTests;

[Trait("Category", "Unit")]
[Trait("Area", "App")]
[Trait("Project", "Projects")]
public class ProjectsModuleTests
{
    [Fact]
    public async Task DefinitionTest()
    {
        var definition = ProjectsModule.Definition;

        await Verify(new { definition }).UseSnapshotsDirectory();
    }
}
