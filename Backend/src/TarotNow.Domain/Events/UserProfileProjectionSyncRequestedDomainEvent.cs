namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu đồng bộ projection profile từ write-model (PG) sang read-model (Mongo).
/// </summary>
public sealed class UserProfileProjectionSyncRequestedDomainEvent : IIdempotentDomainEvent
{
    private const string EventKeyPrefix = "profile:projection-sync";

    /// <summary>
    /// Định danh user cần đồng bộ profile projection.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Display name mới nhất từ write-model.
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Avatar URL mới nhất từ write-model.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Phiên bản nguồn từ write-model để bảo đảm apply theo thứ tự mới nhất.
    /// </summary>
    public DateTime SourceUpdatedAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey => $"{EventKeyPrefix}:{UserId:N}:{SourceUpdatedAtUtc:O}";
}
