using TeamBuild.Core.Domain.Testing;
using TeamBuild.Core.Testing;
using TeamBuild.Projects.Domain.CultureFeature;

namespace TeamBuild.Projects.Domain.UnitTests.CultureFeature;

[UnitTest]
[ProjectsTest]
[DomainLayerTest]
public class CultureJsonContextTests
{
    [Theory]
    [MemberData(nameof(SerializationTestDatas))]
    public async Task SerializationTest(JsonSerializationTestData data)
    {
        await CultureJsonContext.Default.Verify(data);
    }

    public static TheoryData<JsonSerializationTestData> SerializationTestDatas() =>
        [
            // Queries
            new(
                "AvailableCultureListQuery",
                new AvailableCultureListQuery(
                    Filter: new AvailableCultureListQueryFilter(Search: "text filter")
                ),
                typeof(AvailableCultureQuery)
            ),
            new(
                "AvailableCultureListQuerySuccess",
                new AvailableCultureListQuerySuccess(
                    CultureList:
                    [
                        new CultureDetails("es", "Spanish", "Español"),
                        new CultureDetails("en", "English", "English"),
                    ]
                ),
                typeof(AvailableCultureQuerySuccess)
            ),
            new(
                "CultureListQuery",
                new CultureListQuery(Filter: new CultureListQueryFilter(Search: "text filter")),
                typeof(CultureQuery)
            ),
            new(
                "CultureListQuerySuccess",
                new CultureListQuerySuccess(
                    CultureList:
                    [
                        new CultureDetails("es", "Spanish", "Español"),
                        new CultureDetails("en", "English", "English"),
                    ]
                ),
                typeof(CultureQuerySuccess)
            ),
            new(
                "CultureGetByIdQuery",
                new CultureGetByIdQuery(CultureCode: "es"),
                typeof(CultureQuery)
            ),
            new(
                "CultureGetByIdQuerySuccess",
                new CultureGetByIdQuerySuccess(
                    Culture: new CultureDetails("es", "Spanish", "Español")
                ),
                typeof(CultureQuerySuccess)
            ),
            new(
                "CultureGetByIdsQuery",
                new CultureGetByIdsQuery(CultureCodes: ["es", "en"]),
                typeof(CultureQuery)
            ),
            new(
                "CultureGetByIdsQuerySuccess",
                new CultureGetByIdsQuerySuccess(
                    CultureList:
                    [
                        new CultureDetails("es", "Spanish", "Español"),
                        new CultureDetails("en", "English", "English"),
                    ]
                ),
                typeof(CultureQuerySuccess)
            ),
            // Commands
            new(
                "CultureDefineCommand",
                new CultureDefineCommand("es", "Spanish", "Español"),
                typeof(CultureCommand)
            ),
            new(
                "CultureDefineCommandSuccess",
                new CultureDefineCommandSuccess(
                    Culture: new CultureDetails("es", "Spanish", "Español")
                ),
                typeof(CultureCommandSuccess)
            ),
            new("CultureDeleteCommand", new CultureDeleteCommand("es"), typeof(CultureCommand)),
            new(
                "CultureDeleteCommandSuccess",
                new CultureDeleteCommandSuccess(),
                typeof(CultureCommandSuccess)
            ),
        ];

    [Fact]
    public void AllModelsSerializerTested()
    {
        SerializationTestDatas().ShouldBeTestedFor<CultureJsonContext>();
    }

    [Fact]
    public void GetJsonContextsHasContextTest()
    {
        TeamBuildProjectsDomain.GetJsonContexts().ShouldContain(c => c is CultureJsonContext);
    }
}
