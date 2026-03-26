using Microsoft.FeatureManagement;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiObservability(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();
        ConfigureOpenTelemetry(services, configuration);
        return services;
    }

    private static void ConfigureOpenTelemetry(IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"];
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            serviceName = "TarotNow.Api";
        }

        var endpoint = ResolveOtlpEndpoint(configuration);

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                    .AddHttpClientInstrumentation();

                if (endpoint is not null)
                {
                    tracing.AddOtlpExporter(options => options.Endpoint = endpoint);
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                if (endpoint is not null)
                {
                    metrics.AddOtlpExporter(options => options.Endpoint = endpoint);
                }
            });
    }

    private static Uri? ResolveOtlpEndpoint(IConfiguration configuration)
    {
        var endpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return null;
        }

        return Uri.TryCreate(endpoint, UriKind.Absolute, out var parsed) ? parsed : null;
    }
}
