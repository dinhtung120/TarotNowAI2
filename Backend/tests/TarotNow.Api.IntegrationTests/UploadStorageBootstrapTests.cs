using Microsoft.Extensions.Configuration;
using TarotNow.Api.Startup;

namespace TarotNow.Api.IntegrationTests;

public sealed class UploadStorageBootstrapTests
{
    [Fact]
    public void ResolveUploadsPath_ShouldUseConfiguredStorageRoot()
    {
        var storageRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FileStorage:RootPath"] = storageRoot
            })
            .Build();

        var uploadsPath = UploadStorageBootstrapExtensions.ResolveUploadsPath(configuration, "/unused");

        Assert.Equal(Path.Combine(storageRoot, UploadStorageBootstrapExtensions.UploadsFolderName), uploadsPath);
    }

    [Fact]
    public void CreateUploadsFileProvider_ShouldUseBootstrappedDirectory()
    {
        var storageRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FileStorage:RootPath"] = storageRoot
            })
            .Build();
        var uploadsPath = UploadStorageBootstrapExtensions.ResolveUploadsPath(configuration, "/unused");
        Directory.CreateDirectory(uploadsPath);

        var provider = UploadStorageBootstrapExtensions.CreateUploadsFileProvider(configuration, "/unused");

        Assert.True(Directory.Exists(uploadsPath));
        Assert.True(provider.GetDirectoryContents(string.Empty).Exists);
        (provider as IDisposable)?.Dispose();
        Directory.Delete(storageRoot, recursive: true);
    }
}
