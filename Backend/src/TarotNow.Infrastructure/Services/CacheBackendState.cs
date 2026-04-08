
namespace TarotNow.Infrastructure.Services;

// Trạng thái backend cache đang dùng Redis hay fallback in-memory.
public sealed class CacheBackendState
{
    /// <summary>
    /// Khởi tạo trạng thái backend cache theo hạ tầng runtime.
    /// Luồng này giúp tầng diagnostics/health-check biết rõ mode cache hiện tại.
    /// </summary>
    public CacheBackendState(bool usesRedis)
    {
        // Ghi nhận mode cache để các service khác có thể phản ứng theo môi trường.
        UsesRedis = usesRedis;
    }

    // Cờ cho biết hệ thống đang dùng Redis thật hay không.
    public bool UsesRedis { get; }
}
