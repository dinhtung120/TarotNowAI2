namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu admin xử lý withdrawal request.
/// </summary>
public sealed class WithdrawalProcessRequestedDomainEvent : IIdempotentDomainEvent
{
    /// <summary>
    /// Định danh request cần xử lý.
    /// </summary>
    public Guid RequestId { get; init; }

    /// <summary>
    /// Định danh admin thao tác.
    /// </summary>
    public Guid AdminId { get; init; }

    /// <summary>
    /// Action xử lý (approve/reject).
    /// </summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>
    /// Ghi chú admin (bắt buộc khi reject).
    /// </summary>
    public string? AdminNote { get; init; }

    /// <summary>
    /// Idempotency key cho thao tác process.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey
    {
        get
        {
            var normalizedAction = Action?.Trim().ToLowerInvariant() ?? "unknown";
            var normalizedKey = IdempotencyKey?.Trim();
            return $"withdrawal:process:{RequestId:N}:{normalizedAction}:{normalizedKey}";
        }
    }
}
