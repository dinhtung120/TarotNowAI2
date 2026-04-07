using TarotNow.Api.Hubs;
using TarotNow.Api.Middlewares;
using Microsoft.Extensions.FileProviders;

namespace TarotNow.Api.Startup;

public static class ApiApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ChatFeatureGateMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapOpenApi();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        
        app.UseStaticFiles();

        
        
        
        
        var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads");
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }
        
        var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[".avif"] = "image/avif";
        contentTypeProvider.Mappings[".webp"] = "image/webp";

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/uploads",
            ContentTypeProvider = contentTypeProvider,
            ServeUnknownFileTypes = true, 
        });

        app.UseCors();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<ChatHub>("/" + ApiRoutes.ChatHub);
        app.MapHub<PresenceHub>("/api/v1/presence");
        app.MapHub<CallHub>("/" + ApiRoutes.CallHub);

        return app;
    }
}
