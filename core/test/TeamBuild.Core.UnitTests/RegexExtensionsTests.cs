using System.Text.RegularExpressions;
using TeamBuild.Core.Testing;

namespace TeamBuild.Core.UnitTests;

[UnitTest]
[CoreProjectTest]
[PreambleLayerTest]
public class RegexExtensionsTests
{
    [Theory]
    [ClassData(typeof(ExtractGroupTestData))]
    public void ExtractGroupTest(string input, Regex pattern, string groupName, string? expected)
    {
        if (expected is null)
        {
            Should.Throw<InvalidOperationException>(() => input.ExtractGroup(pattern, groupName));
        }
        else
        {
            var actual = input.ExtractGroup(pattern, groupName);

            actual.ShouldBe(expected);
        }
    }

    [Theory]
    [ClassData(typeof(ExtractGroupTestData))]
    public void TryExtractGroupTest(string input, Regex pattern, string groupName, string? expected)
    {
        var result = input.TryExtractGroup(pattern, groupName, out var actual);

        if (expected is null)
        {
            result.ShouldBeFalse();
            actual.ShouldBeNull();
        }
        else
        {
            result.ShouldBeTrue();
            actual.ShouldBe(expected);
        }
    }

    public class ExtractGroupTestData : TheoryData<string, Regex, string, string?>
    {
        public ExtractGroupTestData()
        {
            Add(
                "Version is 3.14 at current date",
                new Regex(@"(?<ver>\d+(\.\d+)+)"),
                "ver",
                "3.14"
            );

            Add("No version in this input", new Regex(@"(?<ver>\d+(\.\d+)+)"), "ver", null);
        }
    }
}
