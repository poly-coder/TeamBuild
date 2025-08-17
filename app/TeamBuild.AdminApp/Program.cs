using JasperFx;
using TeamBuild.AdminApp;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.ConfigureApplication(app);

return await app.RunJasperFxCommands(args);
