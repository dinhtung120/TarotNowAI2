using Microsoft.FeatureManagement;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký khối observability cho API gồm feature management và OpenTelemetry.
    /// Luồng xử lý: bật feature flag service trước, sau đó cấu hình tracing/metrics exporter.
    /// </summary>
    public static IServiceCollection AddApiObservability(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();
        ConfigureOpenTelemetry(services, configuration);
        return services;
    }

    /// <summary>
    /// Cấu hình OpenTelemetry tracing/metrics với endpoint OTLP tùy chọn từ cấu hình.
    /// Luồng xử lý: resolve service name + endpoint, gắn instrumentation, chỉ thêm exporter khi endpoint hợp lệ.
    /// </summary>
    private static void ConfigureOpenTelemetry(IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"];
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            // Edge case không cấu hình tên service: fallback mặc định để telemetry vẫn nhất quán.
            serviceName = "TarotNow.Api";
        }

        var endpoint = ResolveOtlpEndpoint(configuration);

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource("TarotNow.Auth")
                    .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                    .AddHttpClientInstrumentation();

                if (endpoint is not null)
                {
                    // Chỉ bật exporter khi endpoint hợp lệ để tránh lỗi khởi tạo telemetry.
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
                    // Dùng cùng endpoint cho metrics để đồng bộ pipeline observability.
                    metrics.AddOtlpExporter(options => options.Endpoint = endpoint);
                }
            });
    }

    /// <summary>
    /// Resolve endpoint OTLP từ cấu hình, trả null nếu thiếu hoặc sai định dạng.
    /// Luồng xử lý: đọc chuỗi endpoint, validate URI tuyệt đối rồi trả kết quả parse.
    /// </summary>
    private static Uri? ResolveOtlpEndpoint(IConfiguration configuration)
    {
        var endpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            // Không cấu hình OTLP là trường hợp hợp lệ, hệ thống sẽ chạy observability local-only.
            return null;
        }

        return Uri.TryCreate(endpoint, UriKind.Absolute, out var parsed) ? parsed : null;
    }
}
