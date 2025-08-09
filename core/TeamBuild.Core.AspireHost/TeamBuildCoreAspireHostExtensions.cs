using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using TeamBuild.Core.Services;

namespace TeamBuild.Core.AspireHost;

public static class TeamBuildCoreAspireHostExtensions
{
    public static IResourceBuilder<TResource> WithTestingEnvVar<TResource>(
        this IResourceBuilder<TResource> builder
    )
        where TResource : IResourceWithEnvironment
    {
        var isTesting = builder.ApplicationBuilder.Configuration.IsTesting();

        if (isTesting)
        {
            builder.WithEnvironment(TeamBuildCoreServicesExtensions.TestingEnvVarName, "true");
        }

        return builder;
    }
}
