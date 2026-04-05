using Asp.Versioning;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TarotNow.Application;
using TarotNow.Infrastructure;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                /* FIX #24: Dùng CamelCase naming policy cho Enum serialization.
                 * JsonStringEnumConverter() mặc định serialize PascalCase: Audio → "Audio", Video → "Video".
                 * Frontend TypeScript expect lowercase: 'audio', 'video'.
                 * CamelCase policy chuyển thành: Audio → "audio", Video → "video".
                 * Nếu thiếu điều này, session.type === 'video' luôn FALSE → không bật camera. */
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        AddApiVersioningServices(services);
        AddOpenApiServices(services);
        AddPlatformServices(services, configuration);
        AddRateLimitPolicies(services);
        return services;
    }

    private static void AddApiVersioningServices(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddOpenApiServices(IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });
    }
}
