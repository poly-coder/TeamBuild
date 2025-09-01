using TeamBuild.Core.Domain.Testing;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Domain.UnitTests.CultureFeature;

[Trait("Category", "Unit")]
[Trait("Area", "App")]
[Trait("Project", "Projects")]
public class CultureValidationsTests
{
    [Theory]
    [MemberData(nameof(CultureDefineCommandValidatorTestDatas))]
    public async Task CultureDefineCommandValidatorTest(
        ModelValidationTestData<CultureDefineCommand> data
    ) => await new CultureDefineCommandValidator().ShouldBeTestedFor(data, e => e.Coerce());

    public static TheoryData<
        ModelValidationTestData<CultureDefineCommand>
    > CultureDefineCommandValidatorTestDatas() =>
        [
            // Invalid cases
            new("empty", new CultureDefineCommand("", "", "")),
            new("whitespace", new CultureDefineCommand(" \t", " \r", " \n")),
            new(
                "invalid code",
                new CultureDefineCommand("not valid", "Any display name", "is actually valid")
            ),
            // Valid cases
            new("valid", new CultureDefineCommand("es", "Spanish", "Español")),
            new("trimming", new CultureDefineCommand(" es\t", " Spanish\r", " Español\r\n")),
            new("chinese", new CultureDefineCommand("zh-CN", "Chinese (Simplified)", "中文(简体)")),
        ];

    [Theory]
    [MemberData(nameof(CultureDeleteCommandValidatorTestDatas))]
    public async Task CultureDeleteCommandValidatorTest(
        ModelValidationTestData<CultureDeleteCommand> data
    ) => await new CultureDeleteCommandValidator().ShouldBeTestedFor(data, e => e.Coerce());

    public static TheoryData<
        ModelValidationTestData<CultureDeleteCommand>
    > CultureDeleteCommandValidatorTestDatas() =>
        [
            // Invalid cases
            new("empty", new CultureDeleteCommand("")),
            new("whitespace", new CultureDeleteCommand(" \t")),
            new("invalid code", new CultureDeleteCommand("not valid")),
            // Valid cases
            new("valid", new CultureDeleteCommand("es")),
            new("trimming", new CultureDeleteCommand(" es\t")),
            new("chinese", new CultureDeleteCommand("zh-CN")),
        ];
}
