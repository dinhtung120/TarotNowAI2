namespace TarotNow.Infrastructure.Options;

// Options cấu hình root path cho file storage.
public sealed class FileStorageOptions
{
    // Root path filesystem để lưu dữ liệu media dùng chung giữa nhiều instance.
    public string RootPath { get; set; } = string.Empty;
}
