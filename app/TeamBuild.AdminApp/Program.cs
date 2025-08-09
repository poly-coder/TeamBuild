using JasperFx;
using TeamBuild.AdminApp;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup();

startup.ConfigureServices(builder);

var app = builder.Build();

startup.ConfigureApplication(app);

return await app.RunJasperFxCommands(args);
