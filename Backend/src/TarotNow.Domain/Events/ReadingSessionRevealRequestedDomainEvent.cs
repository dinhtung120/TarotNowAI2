using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu reveal một reading session.
/// </summary>
public sealed class ReadingSessionRevealRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng sở hữu phiên.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session id cần reveal.
    /// </summary>
    public string SessionId { get; init; } = string.Empty;

    /// <summary>
    /// Ngôn ngữ ưu tiên cho precompute AI.
    /// </summary>
    public string Language { get; init; } = "vi";

    /// <summary>
    /// Kết quả card sau khi handler xử lý reveal.
    /// </summary>
    public IReadOnlyList<ReadingDrawnCard> RevealedCards { get; set; } = [];

    /// <summary>
    /// Cờ replay khi session đã reveal trước đó.
    /// </summary>
    public bool IsIdempotentReplay { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
