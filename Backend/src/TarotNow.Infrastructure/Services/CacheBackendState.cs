/*
 * FILE: CacheBackendState.cs
 * MỤC ĐÍCH: Object theo dõi trạng thái cache backend (Redis có sẵn hay không).
 *
 *   TẠI SAO CẦN?
 *   → Ứng dụng hỗ trợ 2 chế độ cache: Redis (production) và InMemory (local dev).
 *   → Khi Redis không khả dụng (chưa cài, connection fail) → fallback InMemory.
 *   → CacheBackendState lưu trạng thái hiện tại → các service khác biết đang dùng gì.
 *   → Dùng cho diagnostic endpoint (health check, admin dashboard hiển thị cache status).
 *
 *   SEALED CLASS:
 *   → Không cho phép kế thừa → đảm bảo chỉ có 1 implementation chính xác.
 *   → Immutable: chỉ set UsesRedis trong constructor → không thay đổi sau đó.
 */

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Singleton state object — cho biết cache backend đang dùng Redis hay InMemory.
/// </summary>
public sealed class CacheBackendState
{
    public CacheBackendState(bool usesRedis)
    {
        UsesRedis = usesRedis;
    }

    /// <summary>True nếu đang dùng Redis, false nếu fallback InMemory.</summary>
    public bool UsesRedis { get; }
}
