using Asp.Versioning;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TarotNow.Application;
using TarotNow.Infrastructure;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký nhóm service presentation của API (controller, versioning, OpenAPI, platform, rate-limit).
    /// Luồng xử lý: cấu hình JSON serialization trước, sau đó lần lượt gắn các khối hạ tầng web.
    /// </summary>
    public static IServiceCollection AddApiPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Ép enum ra chuỗi camelCase để payload JSON ổn định cho frontend và swagger schema.
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        AddApiVersioningServices(services);
        AddOpenApiServices(services);
        AddPlatformServices(services, configuration);
        AddRateLimitPolicies(services, configuration);
        return services;
    }

    /// <summary>
    /// Cấu hình API versioning theo URL segment và bật khám phá version cho swagger explorer.
    /// Luồng xử lý: đặt default version, bật report version, rồi cấu hình explorer group format.
    /// </summary>
    private static void AddApiVersioningServices(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                // Khi client không truyền version, fallback về v1 để giữ backward compatibility.
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

    /// <summary>
    /// Đăng ký OpenAPI và nạp XML comments cho Swagger nếu file tài liệu tồn tại.
    /// Luồng xử lý: bật endpoint OpenAPI, cấu hình SwaggerGen, kiểm tra file XML rồi include comments.
    /// </summary>
    private static void AddOpenApiServices(IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            if (File.Exists(xmlPath))
            {
                // Edge case build chưa sinh XML: chỉ include khi file tồn tại để tránh lỗi runtime swagger.
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });
    }
}
