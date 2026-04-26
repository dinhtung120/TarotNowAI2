using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event nhận webhook PayOS để xử lý trạng thái nạp tiền.
/// </summary>
public sealed class DepositWebhookReceivedDomainEvent : IIdempotentDomainEvent
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

    /// <inheritdoc />
    public string EventIdempotencyKey
    {
        get
        {
            var normalizedPayload = RawPayload?.Trim() ?? string.Empty;
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalizedPayload));
            var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 16)).ToLowerInvariant();
            return $"deposit:webhook:{hashPrefix}";
        }
    }
}
