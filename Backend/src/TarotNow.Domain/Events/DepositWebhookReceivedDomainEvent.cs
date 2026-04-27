using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event nhận webhook PayOS để xử lý trạng thái nạp tiền.
/// </summary>
public sealed partial class DepositWebhookReceivedDomainEvent : IIdempotentDomainEvent
{
    private const string EventKeyPrefix = "deposit:webhook";
    private const string ProviderName = "payos";

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
            var providerIdentityKey = TryBuildProviderIdentityKey(RawPayload);
            if (!string.IsNullOrWhiteSpace(providerIdentityKey))
            {
                return providerIdentityKey;
            }

            var normalizedPayload = CanonicalizePayload(RawPayload);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalizedPayload));
            var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 16)).ToLowerInvariant();
            return $"{EventKeyPrefix}:payload:{hashPrefix}";
        }
    }
}
