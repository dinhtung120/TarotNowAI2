using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;
using TarotNow.Api.Middlewares;
using TarotNow.Api.Realtime;
using TarotNow.Api.Services;
using TarotNow.Application;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Infrastructure;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddApiPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
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

    private static void AddPlatformServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApplicationServices(Assembly.GetExecutingAssembly());
        services.AddInfrastructureServices(configuration);
        services.AddApiObservability(configuration);
        services.AddScoped<IRefreshTokenCookieService, RefreshTokenCookieService>();
        services.AddSingleton<IUserPresenceTracker, InMemoryUserPresenceTracker>();
        services.AddAuthorization();
        services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB for media metadata payload (base64/data-url).
        });
    }

    private static void AddRateLimitPolicies(IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    var retryAfterSeconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
                    context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString(CultureInfo.InvariantCulture);
                }

                await context.HttpContext.Response.WriteAsJsonAsync(
                    new { error = "TOO_MANY_REQUESTS", message = "Too many requests. Please try again later." },
                    cancellationToken: token);
            };

            options.AddPolicy("login", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ResolveClientIp(httpContext),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(60),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                        AutoReplenishment = true
                    }));
        });
    }

}
