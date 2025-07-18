var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TeamBuild_AdminApp>("adminapp");

builder.Build().Run();
