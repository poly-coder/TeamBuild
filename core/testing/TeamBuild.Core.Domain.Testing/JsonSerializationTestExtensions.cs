using Shouldly;

namespace TeamBuild.Core.Domain.Testing;

public static class JsonSerializationTestExtensions
{
    public static void ShouldBeTestedFor<TReferenceType>(
        this IEnumerable<JsonSerializationTestData> testData
    ) => testData.ShouldBeTestedFor(typeof(TReferenceType));

    public static void ShouldBeTestedFor(
        this IEnumerable<JsonSerializationTestData> testData,
        Type namespacedType
    )
    {
        var modelTypes = namespacedType
            .Assembly.ExportedTypes.Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t => t.Namespace == namespacedType.Namespace)
            .Where(t => t.IsAssignableTo(typeof(IDomainMessage)))
            .Select(t => t.FullName ?? t.Name)
            .OrderBy(e => e)
            .ToList();

        var testedInputTypes = testData
            .Select(d => d.Input.GetType())
            .Select(t => t.FullName ?? t.Name)
            .Distinct()
            .OrderBy(e => e)
            .ToList();

        var missingTypes = modelTypes.Except(testedInputTypes).ToList();
        var unnecessaryTypes = testedInputTypes.Except(modelTypes).ToList();

        missingTypes.ShouldSatisfyAllConditions(
            () => missingTypes.ShouldBeEmpty(),
            () => unnecessaryTypes.ShouldBeEmpty()
        );
    }
}
