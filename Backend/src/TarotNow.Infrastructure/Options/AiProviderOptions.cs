namespace TarotNow.Infrastructure.Options;

// Options cấu hình kết nối AI provider.
public sealed class AiProviderOptions
{
    // API key dùng để xác thực với nhà cung cấp AI.
    public string ApiKey { get; set; } = string.Empty;

    // Base URL endpoint gọi API AI.
    public string BaseUrl { get; set; } = string.Empty;

    // Model mặc định dùng cho yêu cầu AI.
    public string Model { get; set; } = string.Empty;

    // Timeout request AI (giây).
    public int TimeoutSeconds { get; set; } = 30;

    // Số lần retry tối đa khi request AI lỗi tạm thời.
    public int MaxRetries { get; set; } = 2;
}
