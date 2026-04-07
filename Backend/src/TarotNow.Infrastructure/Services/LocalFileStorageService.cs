using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<LocalFileStorageService> _logger;

    public LocalFileStorageService(IWebHostEnvironment env, ILogger<LocalFileStorageService> logger)
    {
        _env = env;
        _logger = logger;
    }

        private string GetStorageRoot()
    {
        return Path.Combine(_env.ContentRootPath, "wwwroot");
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder, CancellationToken ct = default)
    {
        try
        {
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
            
            var relativePath = Path.Combine("uploads", subfolder, uniqueFileName).Replace("\\", "/");
            var absolutePath = Path.Combine(GetStorageRoot(), "uploads", subfolder, uniqueFileName);

            var directory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            using var fileStreamToWrite = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await fileStream.CopyToAsync(fileStreamToWrite, ct);

            return $"/{relativePath}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lưu file {FileName} vào subfolder {Subfolder}", fileName, subfolder);
            throw;
        }
    }

    public Task DeleteFileAsync(string relativePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || relativePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        try
        {
            var normalizedRelativePath = relativePath.TrimStart('/');
            var absolutePath = Path.Combine(GetStorageRoot(), normalizedRelativePath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
                _logger.LogInformation("Đã xóa file tĩnh: {AbsolutePath}", absolutePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa file {RelativePath}", relativePath);
            
        }

        return Task.CompletedTask;
    }
}
