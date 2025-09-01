using TeamBuild.Core.Testing;

namespace TeamBuild.Core.Domain.UnitTests;

[UnitTest]
[CoreProjectTest]
[DomainLayerTest]
public class DefinitionKeyTests
{
    [Theory]
    [MemberData(nameof(CreateTestDatas))]
    public void TryCreateTest(CreateTestData data)
    {
        var actual = DefinitionKey.TryCreate(data.Parts, out var key);

        actual.ShouldBe(data.ExpectedResult);

        if (actual)
        {
            key.ShouldNotBeNull();
            key.Parts.ToList().ShouldBeEquivalentTo(data.Parts);
            key.Id.ShouldBe(data.Id);
            key.FullName.ShouldBe(data.FullName);
            key.Name.ShouldBe(data.Name);
            key.Part.ShouldBe(data.Parts[^1]);
            string id = key;
            id.ShouldBe(data.Id);
            var parsedKey = (DefinitionKey)data.Id;
            parsedKey.ShouldBe(key);
        }
        else
        {
            key.ShouldBeNull();
        }
    }

    [Theory]
    [MemberData(nameof(CreateTestDatas))]
    public void CreateTest(CreateTestData data)
    {
        DefinitionKey Action() => DefinitionKey.Create(data.Parts);

        if (data.ExpectedResult)
        {
            var key = Action();
            key.ShouldNotBeNull();
            key.Parts.ToList().ShouldBeEquivalentTo(data.Parts);
            key.Id.ShouldBe(data.Id);
            key.FullName.ShouldBe(data.FullName);
            key.Name.ShouldBe(data.Name);
            key.Part.ShouldBe(data.Parts[^1]);
            string id = key;
            id.ShouldBe(data.Id);
            var parsedKey = (DefinitionKey)data.Id;
            parsedKey.ShouldBe(key);
        }
        else
        {
            Should.Throw<ArgumentException>(Action);
        }
    }

    public record CreateTestData(
        List<DefinitionKeyPart> Parts,
        bool ExpectedResult,
        string Id,
        string FullName,
        string Name
    );

    public static TheoryData<CreateTestData> CreateTestDatas()
    {
        var module = (DefinitionKeyPart)"M:module";
        var resource = (DefinitionKeyPart)"R:resource";
        var operation = (DefinitionKeyPart)"O:operation";

        return
        [
            // valid data
            new CreateTestData([module], true, "M:module.", "module", "module"),
            new CreateTestData(
                [module, resource],
                true,
                "M:module.R:resource.",
                "module.resource",
                "resource"
            ),
            new CreateTestData(
                [module, resource, operation],
                true,
                "M:module.R:resource.O:operation.",
                "module.resource.operation",
                "operation"
            ),
            // invalid data
            new CreateTestData(null!, false, "", "", ""),
            new CreateTestData([module, null!, operation], false, "", "", ""),
        ];
    }

    [Theory]
    [MemberData(nameof(EqualityTestDatas))]
    public void EqualityTest(EqualityTestData data)
    {
        var key1 = DefinitionKey.Create(data.Parts);
        var key2 = DefinitionKey.Create(data.Parts);

        key1.ShouldBe(key2);
        key1.Equals(key2).ShouldBeTrue();
        key1.GetHashCode().ShouldBe(key2.GetHashCode());

        key1.Equals(null).ShouldBeFalse();
        key1!.Equals(key1).ShouldBeTrue();
        // ReSharper disable once SuspiciousTypeConversion.Global
        key1.Equals(data.Parts).ShouldBeFalse();

        (key1 == key2).ShouldBeTrue();
        (key1 != key2).ShouldBeFalse();
    }

    public record EqualityTestData(List<DefinitionKeyPart> Parts);

    public static TheoryData<EqualityTestData> EqualityTestDatas()
    {
        var module = (DefinitionKeyPart)"M:module";
        var resource = (DefinitionKeyPart)"R:resource";
        var operation = (DefinitionKeyPart)"O:operation";

        return
        [
            new EqualityTestData([module]),
            new EqualityTestData([module, resource]),
            new EqualityTestData([module, resource, operation]),
        ];
    }

    [Theory]
    [MemberData(nameof(ParseTestDatas))]
    public void TryParseTest(ParseTestData data)
    {
        var actual = DefinitionKey.TryParse(data.Id, out var key);

        actual.ShouldBe(data.ExpectedResult);

        if (actual)
        {
            key.ShouldNotBeNull();
            key.Parts.ToList().ShouldBeEquivalentTo(data.Parts);
            key.Id.ShouldBe(data.Id);
            key.Part.ShouldBe(data.Parts[^1]);
            string id = key;
            id.ShouldBe(data.Id);
            var parsedKey = (DefinitionKey)data.Id;
            parsedKey.ShouldBe(key);
        }
        else
        {
            key.ShouldBeNull();
        }
    }

    [Theory]
    [MemberData(nameof(ParseTestDatas))]
    public void ParseTest(ParseTestData data)
    {
        DefinitionKey Action() => DefinitionKey.Parse(data.Id);

        if (data.ExpectedResult)
        {
            var key = Action();
            key.ShouldNotBeNull();
            key.Parts.ToList().ShouldBeEquivalentTo(data.Parts);
            key.Id.ShouldBe(data.Id);
            key.Part.ShouldBe(data.Parts[^1]);
            string id = key;
            id.ShouldBe(data.Id);
            var parsedKey = (DefinitionKey)data.Id;
            parsedKey.ShouldBe(key);
        }
        else
        {
            Should.Throw<ArgumentException>(Action);
        }
    }

    public record ParseTestData(string Id, bool ExpectedResult, List<DefinitionKeyPart> Parts);

    public static TheoryData<ParseTestData> ParseTestDatas()
    {
        var module = (DefinitionKeyPart)"M:module";
        var resource = (DefinitionKeyPart)"R:resource";
        var operation = (DefinitionKeyPart)"O:operation";

        return
        [
            // valid data
            new ParseTestData("M:module.", true, [module]),
            new ParseTestData("M:module.R:resource.", true, [module, resource]),
            new ParseTestData(
                "M:module.R:resource.O:operation.",
                true,
                [module, resource, operation]
            ),
            // invalid data
            new ParseTestData(null!, false, []),
            new ParseTestData("", false, []),
            new ParseTestData(" \t\r\n", false, []),
            new ParseTestData("M:module", false, []),
            new ParseTestData("M;module.", false, []),
            new ParseTestData("X:module.", false, []),
        ];
    }
}
