using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

// Service lưu/xóa file local trong thư mục tĩnh của backend.
public class LocalFileStorageService : IFileStorageService
{
    // Môi trường host để xác định content root runtime.
    private readonly IWebHostEnvironment _env;
    // Logger theo dõi thao tác lưu/xóa file.
    private readonly ILogger<LocalFileStorageService> _logger;
    // Cấu hình root path tùy chọn cho shared storage (NFS/EFS...).
    private readonly FileStorageOptions _options;

    /// <summary>
    /// Khởi tạo service lưu trữ file local.
    /// Luồng inject môi trường và logger để thao tác filesystem có thể truy vết.
    /// </summary>
    public LocalFileStorageService(
        IWebHostEnvironment env,
        ILogger<LocalFileStorageService> logger,
        IOptions<FileStorageOptions> options)
    {
        _env = env;
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Lấy thư mục gốc lưu file tĩnh của ứng dụng.
    /// Luồng tách helper để đảm bảo mọi thao tác dùng chung một root path.
    /// </summary>
    private string GetStorageRoot()
    {
        if (!string.IsNullOrWhiteSpace(_options.RootPath))
        {
            return _options.RootPath;
        }

        return Path.Combine(_env.ContentRootPath, "wwwroot");
    }

    /// <summary>
    /// Lưu file vào thư mục uploads và trả về đường dẫn tương đối để public.
    /// Luồng tạo tên file ngẫu nhiên, đảm bảo thư mục tồn tại rồi ghi stream bất đồng bộ.
    /// </summary>
    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subfolder, CancellationToken ct = default)
    {
        try
        {
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid():N}{extension}";

            // Tách relative/absolute path để vừa ghi file vừa trả URL tương đối cho client.
            var relativePath = Path.Combine("uploads", subfolder, uniqueFileName).Replace("\\", "/");
            var absolutePath = Path.Combine(GetStorageRoot(), "uploads", subfolder, uniqueFileName);

            var directory = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(directory))
            {
                // Tạo thư mục khi chưa tồn tại để tránh lỗi ghi file lần đầu.
                Directory.CreateDirectory(directory!);
            }

            // Ghi file theo chế độ async để giảm block thread khi upload lớn.
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

    /// <summary>
    /// Xóa file local theo đường dẫn tương đối.
    /// Luồng bỏ qua URL ngoài và path rỗng để tránh xóa nhầm ngoài phạm vi lưu trữ nội bộ.
    /// </summary>
    public Task DeleteFileAsync(string relativePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || relativePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            // Edge case: dữ liệu ngoài hệ thống local thì bỏ qua an toàn.
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
            // Chủ động nuốt lỗi xóa file để không làm hỏng luồng nghiệp vụ chính.
        }

        return Task.CompletedTask;
    }
}
