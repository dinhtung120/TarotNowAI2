namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu tạo một withdrawal request mới.
/// </summary>
public sealed class WithdrawalCreateRequestedDomainEvent : IIdempotentDomainEvent
{
    /// <summary>
    /// Định danh user gửi yêu cầu.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Số diamond user muốn rút.
    /// </summary>
    public long AmountDiamond { get; init; }

    /// <summary>
    /// Idempotency key phía client.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Ghi chú bổ sung từ user.
    /// </summary>
    public string? UserNote { get; init; }

    /// <summary>
    /// Id request được tạo sau xử lý.
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Trạng thái request sau xử lý.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey
    {
        get
        {
            var normalized = IdempotencyKey?.Trim();
            return $"withdrawal:create:{UserId:N}:{normalized}";
        }
    }
}
