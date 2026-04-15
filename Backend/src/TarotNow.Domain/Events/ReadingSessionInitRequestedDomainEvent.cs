using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu khởi tạo reading session.
/// </summary>
public sealed class ReadingSessionInitRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng mở phiên.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Loại spread yêu cầu.
    /// </summary>
    public string SpreadType { get; init; } = string.Empty;

    /// <summary>
    /// Câu hỏi của người dùng.
    /// </summary>
    public string? Question { get; init; }

    /// <summary>
    /// Currency đầu vào từ client.
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Session id được tạo từ handler.
    /// </summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Chi phí gold được resolve.
    /// </summary>
    public long CostGold { get; set; }

    /// <summary>
    /// Chi phí diamond được resolve.
    /// </summary>
    public long CostDiamond { get; set; }

    /// <summary>
    /// Currency normalize cuối.
    /// </summary>
    public string CurrencyUsed { get; set; } = string.Empty;

    /// <summary>
    /// Số tiền cần charge tại reveal.
    /// </summary>
    public long AmountCharged { get; set; }

    /// <summary>
    /// Snapshot session đã tạo.
    /// </summary>
    public ReadingSession? Session { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
