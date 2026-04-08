namespace TarotNow.Infrastructure.Options;

// Options cấu hình danh sách origin được phép gọi API.
public sealed class CorsOptions
{
    // Danh sách origin hợp lệ cho chính sách CORS.
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}
