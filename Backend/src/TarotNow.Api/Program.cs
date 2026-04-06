using TarotNow.Api.Startup;
using Serilog;

EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services
    .AddApiPresentationServices(builder.Configuration)
    .AddConfiguredCors(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseApiPipeline();
app.Run();

public partial class Program { }
