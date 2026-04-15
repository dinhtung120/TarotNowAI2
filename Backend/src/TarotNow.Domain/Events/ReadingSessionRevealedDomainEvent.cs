using TarotNow.Domain.Entities;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh sau khi một reading session được reveal thành công.
/// </summary>
public sealed class ReadingSessionRevealedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng sở hữu session.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session id đã reveal.
    /// </summary>
    public string SessionId { get; init; } = string.Empty;

    /// <summary>
    /// Ngôn ngữ ưu tiên cho AI precompute.
    /// </summary>
    public string Language { get; init; } = "vi";

    /// <summary>
    /// Snapshot cards đã reveal.
    /// </summary>
    public IReadOnlyList<ReadingDrawnCard> RevealedCards { get; init; } = [];

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
