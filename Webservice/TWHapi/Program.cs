using TWHapi.ProgramHelpers.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .InitializeCORS(builder.Configuration)
    .InitializeServices(builder.Configuration)
    .InitializeOptions();

builder.Logging.InitializeLogging();

builder
    .Build()
    .CreateHostBuilder(builder.Environment, builder.Configuration)
    .Configure()
    .Run();


