using System.Reflection;
using TeamBuild.Core.Testing;

namespace TeamBuild.Core.UnitTests;

[UnitTest]
[CoreProjectTest]
[PreambleLayerTest]
public class AssemblyExtensionsTests
{
    [Theory]
    [ClassData(typeof(GetAssemblyNameTestData))]
    public void GetAssemblyNameTest(Assembly assembly, string expected)
    {
        // Act
        var actual = assembly.GetAssemblyName();

        // Assert
        actual.ShouldBe(expected);
    }

    public class GetAssemblyNameTestData : TheoryData<Assembly, string>
    {
        public GetAssemblyNameTestData()
        {
            Add(typeof(IEnumerable<>).Assembly, "System.Private.CoreLib");
            Add(typeof(Should).Assembly, "Shouldly");
            Add(typeof(AssemblyExtensions).Assembly, "TeamBuild.Core");
        }
    }

    [Theory]
    [ClassData(typeof(GetAssemblyVersionTestData))]
    public void GetAssemblyVersionTest(Assembly assembly, string expected)
    {
        // Act
        var actual = assembly.GetAssemblyVersion();

        // Assert
        actual.ShouldBe(expected);
    }

    public class GetAssemblyVersionTestData : TheoryData<Assembly, string>
    {
        public GetAssemblyVersionTestData()
        {
            Add(typeof(IEnumerable<>).Assembly, "9.0.0");
            Add(typeof(Should).Assembly, "4.3.0");
            Add(typeof(AssemblyExtensions).Assembly, "0.1.0");
        }
    }
}
