namespace TeamBuild.Core.Domain.UnitTests;

[Trait("Category", "Unit")]
[Trait("Area", "Core")]
[Trait("Project", "Domain")]
public class DefinitionKeyPartTests
{
    [Theory]
    [ClassData(typeof(CreateTestData))]
    public void TryCreateTest(DefinitionKeyPartType type, string name, bool expectedResult)
    {
        var actual = DefinitionKeyPart.TryCreate(type, name, out var part);

        actual.ShouldBe(expectedResult);

        if (actual)
        {
            part.ShouldNotBeNull();
            part.Type.ShouldBe(type);
            part.Name.ShouldBe(name);
        }
        else
        {
            part.ShouldBeNull();
        }
    }

    [Theory]
    [ClassData(typeof(CreateTestData))]
    public void CreateTest(DefinitionKeyPartType type, string name, bool expectedResult)
    {
        DefinitionKeyPart Action() => DefinitionKeyPart.Create(type, name);

        if (expectedResult)
        {
            var part = Action();
            var (actualType, actualName) = part;
            part.ShouldNotBeNull();
            part.Type.ShouldBe(type);
            part.Name.ShouldBe(name);
            actualType.ShouldBe(type);
            actualName.ShouldBe(name);
        }
        else
        {
            Should.Throw<ArgumentException>(Action);
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public class CreateTestData : TheoryData<DefinitionKeyPartType, string, bool>
    {
        public CreateTestData()
        {
            // valid
            Add(DefinitionKeyPartType.Module, "module", true);
            Add(DefinitionKeyPartType.Resource, "sub-resource", true);
            Add(DefinitionKeyPartType.Operation, "get_one", true);
            Add(DefinitionKeyPartType.Operation, "get1_2_or3", true);

            // null or white-space names
            Add(DefinitionKeyPartType.Module, null!, false);
            Add(DefinitionKeyPartType.Module, "", false);
            Add(DefinitionKeyPartType.Module, " ", false);
            Add(DefinitionKeyPartType.Module, "\t", false);
            Add(DefinitionKeyPartType.Module, "\r\n", false);

            // invalid patterns
            Add(DefinitionKeyPartType.Module, "123abc", false);
            Add(DefinitionKeyPartType.Module, "1_or_2", false);
            Add(DefinitionKeyPartType.Module, "get many", false);
            Add(DefinitionKeyPartType.Module, "get__one", false);
            Add(DefinitionKeyPartType.Module, "get-_-many", false);

            // invalid types
            Add((DefinitionKeyPartType)(-1), "module", false);
        }
    }

    [Theory]
    [ClassData(typeof(EqualityTestData))]
    public void EqualityTest(DefinitionKeyPartType type, string name)
    {
        var part1 = DefinitionKeyPart.Create(type, name);
        var part2 = DefinitionKeyPart.Create(type, name);

        part1.ShouldBe(part2);
        part1.Equals(part2).ShouldBeTrue();
        part1.GetHashCode().ShouldBe(part2.GetHashCode());

        part1.Equals(null).ShouldBeFalse();
        part1!.Equals(part1).ShouldBeTrue();
        // ReSharper disable once SuspiciousTypeConversion.Global
        part1.Equals(name).ShouldBeFalse();

        (part1 == part2).ShouldBeTrue();
        (part1 != part2).ShouldBeFalse();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public class EqualityTestData : TheoryData<DefinitionKeyPartType, string>
    {
        public EqualityTestData()
        {
            // valid
            Add(DefinitionKeyPartType.Module, "module");
            Add(DefinitionKeyPartType.Resource, "sub-resource");
            Add(DefinitionKeyPartType.Operation, "get_one");
            Add(DefinitionKeyPartType.Operation, "get1_2_or3");
        }
    }

    [Theory]
    [ClassData(typeof(ToStringTestData))]
    public void ToStringTest(DefinitionKeyPartType type, string name, string expected)
    {
        var part = DefinitionKeyPart.Create(type, name);

        part.ToString().ShouldBe(expected);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public class ToStringTestData : TheoryData<DefinitionKeyPartType, string, string>
    {
        public ToStringTestData()
        {
            // valid
            Add(DefinitionKeyPartType.Module, "module", "M:module");
            Add(DefinitionKeyPartType.Resource, "sub-resource", "R:sub-resource");
            Add(DefinitionKeyPartType.Operation, "get_one", "O:get_one");
            Add(DefinitionKeyPartType.Operation, "get1_2_or3", "O:get1_2_or3");
        }
    }

    [Theory]
    [ClassData(typeof(ParseTestData))]
    public void ParseTest(string text, bool expected, DefinitionKeyPartType type, string? name)
    {
        DefinitionKeyPart Action() => DefinitionKeyPart.Parse(text);

        if (expected)
        {
            var part = Action();
            part.Type.ShouldBe(type);
            part.Name.ShouldBe(name);
        }
        else
        {
            Should.Throw<ArgumentException>(Action);
        }
    }

    [Theory]
    [ClassData(typeof(ParseTestData))]
    public void TryParseTest(string text, bool expected, DefinitionKeyPartType type, string? name)
    {
        var result = DefinitionKeyPart.TryParse(text, out var actual);

        result.ShouldBe(expected);

        if (result)
        {
            actual.ShouldNotBeNull();
            actual.Type.ShouldBe(type);
            actual.Name.ShouldBe(name);
        }
        else
        {
            actual.ShouldBeNull();
        }
    }

    public class ParseTestData : TheoryData<string, bool, DefinitionKeyPartType, string?>
    {
        public ParseTestData()
        {
            // valid
            Add("M:module", true, DefinitionKeyPartType.Module, "module");
            Add("R:sub-resource", true, DefinitionKeyPartType.Resource, "sub-resource");
            Add("O:get_one", true, DefinitionKeyPartType.Operation, "get_one");

            // invalid
            Add(null!, false, DefinitionKeyPartType.Module, null);
            Add("", false, DefinitionKeyPartType.Module, null);
            Add("M", false, DefinitionKeyPartType.Module, null);
            Add("M:", false, DefinitionKeyPartType.Module, null);
            Add("M: ", false, DefinitionKeyPartType.Module, null);
            Add("M:module ", false, DefinitionKeyPartType.Module, null);
            Add(":module ", false, DefinitionKeyPartType.Module, null);
            Add("X:module", false, DefinitionKeyPartType.Module, null);
        }
    }
}
