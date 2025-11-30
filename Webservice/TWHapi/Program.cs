using Core.BackgroundProcessing;
using TWHapi.ProgramHelpers.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .InitializeCORS(builder.Configuration)
    .InitializeServices(builder.Configuration)
    .InitializeOptions(builder.Configuration);

builder.Logging.InitializeLogging();
builder.Services.AddHostedService<BackgroundWorker>();

builder
    .Build()
    .CreateHostBuilder(builder.Environment, builder.Configuration)
    .Configure()
    .Middlewares() // Register middlewares should be after Configure() and before ConfigureTail()
    .ConfigureTail()
    .Run();


