using Microsoft.Extensions.FileProviders;

namespace TarotNow.Api.Startup;

public static class UploadStorageBootstrapExtensions
{
    public const string UploadsFolderName = "uploads";
    public const string WebRootFolderName = "wwwroot";

    public static Task EnsureUploadStorageIsReadyAsync(this WebApplication app)
    {
        var uploadsPath = ResolveUploadsPath(app.Configuration, app.Environment.ContentRootPath);
        app.Logger.LogInformation("Ensuring upload storage directory exists. UploadsPath={UploadsPath}", uploadsPath);
        Directory.CreateDirectory(uploadsPath);
        return Task.CompletedTask;
    }

    public static string ResolveUploadsPath(IConfiguration configuration, string contentRootPath)
    {
        var configuredStorageRoot = configuration["FileStorage:RootPath"]?.Trim();
        var storageRoot = string.IsNullOrWhiteSpace(configuredStorageRoot)
            ? Path.Combine(contentRootPath, WebRootFolderName)
            : configuredStorageRoot;
        return Path.Combine(storageRoot, UploadsFolderName);
    }

    public static IFileProvider CreateUploadsFileProvider(IConfiguration configuration, string contentRootPath)
    {
        return new PhysicalFileProvider(ResolveUploadsPath(configuration, contentRootPath));
    }
}
