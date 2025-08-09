using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using TeamBuild.Core.Services;

namespace TeamBuild.Core.AspirePostgres;

public static class TeamBuildCoreAspirePostgresExtensions
{
    public static IResourceBuilder<PostgresServerResource> AddTeamPostgres(
        this IDistributedApplicationBuilder builder,
        string name = "postgres"
    )
    {
        var martenServer = builder.AddPostgres(name);

        if (!builder.Configuration.IsTesting())
            martenServer
                .WithPgAdmin()
                .WithPgWeb()
                .WithDataVolume()
                .WithLifetime(ContainerLifetime.Persistent);

        return martenServer;
    }

    public static IResourceBuilder<PostgresDatabaseResource> AddTeamDatabase(
        this IResourceBuilder<PostgresServerResource> server,
        string name = "martendb",
        string databaseName = "postgres"
    )
    {
        return server.AddDatabase(name: name, databaseName: databaseName);
    }
}
