using TeamBuild.Core.AspireHost;
using TeamBuild.Core.AspirePostgres;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddTeamPostgres();
var database = postgres.AddTeamDatabase();

builder
    .AddProject<Projects.TeamBuild_AdminApp>("adminapp")
    .WithReference(database)
    .WaitFor(database)
    .WithTestingEnvVar();

await builder.Build().RunAsync();
