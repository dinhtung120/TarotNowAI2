namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi người dùng được cấp title.
/// </summary>
public sealed class TitleGrantedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng nhận title.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã title được cấp.
    /// </summary>
    public string TitleCode { get; init; } = string.Empty;

    /// <summary>
    /// Nguồn nghiệp vụ cấp title.
    /// </summary>
    public string Source { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
