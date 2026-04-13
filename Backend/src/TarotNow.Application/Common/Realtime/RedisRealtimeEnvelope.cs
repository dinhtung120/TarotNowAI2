using System.Text.Json;

namespace TarotNow.Application.Common.Realtime;

/// <summary>
/// Envelope chuẩn hóa payload publish qua Redis Pub/Sub.
/// </summary>
public sealed class RedisRealtimeEnvelope
{
    /// <summary>
    /// Tên event phía client sẽ nhận.
    /// </summary>
    public string EventName { get; init; } = string.Empty;

    /// <summary>
    /// Payload JSON của event.
    /// </summary>
    public JsonElement Payload { get; init; }

    /// <summary>
    /// Thời điểm tạo message theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
