namespace TarotNow.Application.Interfaces;

// Abstraction kiểm tra readiness dependency phục vụ endpoint health.
public interface IReadinessService
{
    /// <summary>
    /// Kiểm tra trạng thái sẵn sàng của dependency chính.
    /// </summary>
    Task<ReadinessStatus> CheckAsync(CancellationToken cancellationToken = default);
}

// Snapshot kết quả readiness của các dependency chính.
public sealed record ReadinessStatus(
    bool PostgreSqlReady,
    bool MongoDbReady,
    bool RedisReady);
