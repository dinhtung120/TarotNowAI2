using TarotNow.Api.Hubs;
using TarotNow.Api.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

namespace TarotNow.Api.Startup;

public static class ApiApplicationBuilderExtensions
{
    private const string UploadsFolderName = "uploads";
    private const string WebRootFolderName = "wwwroot";
    private const string PresenceHubPath = "/api/v1/presence";
    private const string AvifExtension = ".avif";
    private const string AvifMimeType = "image/avif";
    private const string WebpExtension = ".webp";
    private const string WebpMimeType = "image/webp";

    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        ConfigureErrorAndRequestMiddlewares(app);
        ConfigureSwagger(app);
        ConfigureHttps(app);
        ConfigureStaticFiles(app);
        ConfigureSecurityPipeline(app);
        MapRealtimeEndpoints(app);
        app.MapControllers();

        return app;
    }

    private static void ConfigureErrorAndRequestMiddlewares(WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ChatFeatureGateMiddleware>();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        app.MapOpenApi();

        if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void ConfigureHttps(WebApplication app)
    {
        if (app.Environment.IsDevelopment()) return;
        app.UseHttpsRedirection();
    }

    private static void ConfigureStaticFiles(WebApplication app)
    {
        app.UseStaticFiles();

        var uploadsPath = EnsureUploadsDirectory(app);
        var contentTypeProvider = CreateUploadsContentTypeProvider();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/" + UploadsFolderName,
            ContentTypeProvider = contentTypeProvider,
            ServeUnknownFileTypes = true,
        });
    }

    private static string EnsureUploadsDirectory(WebApplication app)
    {
        var uploadsPath = Path.Combine(app.Environment.ContentRootPath, WebRootFolderName, UploadsFolderName);
        if (Directory.Exists(uploadsPath)) return uploadsPath;
        Directory.CreateDirectory(uploadsPath);
        return uploadsPath;
    }

    private static FileExtensionContentTypeProvider CreateUploadsContentTypeProvider()
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[AvifExtension] = AvifMimeType;
        contentTypeProvider.Mappings[WebpExtension] = WebpMimeType;
        return contentTypeProvider;
    }

    private static void ConfigureSecurityPipeline(WebApplication app)
    {
        app.UseCors();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static void MapRealtimeEndpoints(WebApplication app)
    {
        app.MapHub<ChatHub>("/" + ApiRoutes.ChatHub);
        app.MapHub<PresenceHub>(PresenceHubPath);
        app.MapHub<CallHub>("/" + ApiRoutes.CallHub);
    }
}
