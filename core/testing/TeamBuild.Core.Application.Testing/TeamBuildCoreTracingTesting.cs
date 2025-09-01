using Shouldly;
using TeamBuild.Core.Domain;

namespace TeamBuild.Core.Application.Testing;

public static class TeamBuildCoreTracingTesting
{
    public static void ShouldHaveOperationTags(
        this IEnumerable<KeyValuePair<string, object?>> source,
        string? expectedProject,
        string? expectedLayer,
        string? expectedEntity,
        string? expectedOperation
    )
    {
        source.ShouldNotBeNull();

        var tags = source.ToArray();

        tags.ShouldSatisfyAllConditions(
            () =>
            {
                tags.ShouldContain(
                    KeyValuePair.Create(
                        TeamBuildCoreDomain.AppSystemTagKey,
                        (object?)TeamBuildCoreDomain.TeamBuildSystemName
                    )
                );
            },
            () =>
            {
                if (expectedProject is not null)
                {
                    tags.ShouldContain(
                        KeyValuePair.Create(
                            TeamBuildCoreDomain.AppProjectTagKey,
                            (object?)expectedProject
                        )
                    );
                }
            },
            () =>
            {
                if (expectedLayer is not null)
                {
                    tags.ShouldContain(
                        KeyValuePair.Create(
                            TeamBuildCoreDomain.AppLayerTagKey,
                            (object?)expectedLayer
                        )
                    );
                }
            },
            () =>
            {
                if (expectedEntity is not null)
                {
                    tags.ShouldContain(
                        KeyValuePair.Create(
                            TeamBuildCoreDomain.AppEntityTagKey,
                            (object?)expectedEntity
                        )
                    );
                }
            },
            () =>
            {
                if (expectedOperation is not null)
                {
                    tags.ShouldContain(
                        KeyValuePair.Create(
                            TeamBuildCoreDomain.AppOperationTagKey,
                            (object?)expectedOperation
                        )
                    );
                }
            }
        );
    }
}
