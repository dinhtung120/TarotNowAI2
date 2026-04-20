namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event nhận webhook PayOS để xử lý trạng thái nạp tiền.
/// </summary>
public sealed class DepositWebhookReceivedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Payload raw nhận từ PayOS.
    /// </summary>
    public string RawPayload { get; init; } = string.Empty;

    /// <summary>
    /// Cờ đánh dấu đã xử lý webhook hợp lệ.
    /// </summary>
    public bool Handled { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
