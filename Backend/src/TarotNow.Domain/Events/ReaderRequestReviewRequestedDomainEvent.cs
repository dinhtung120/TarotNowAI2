namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu admin duyệt hoặc từ chối đơn Reader.
/// </summary>
public sealed class ReaderRequestReviewRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh đơn Reader cần xử lý.
    /// </summary>
    public string RequestId { get; init; } = string.Empty;

    /// <summary>
    /// Hành động xử lý (approve/reject).
    /// </summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>
    /// Ghi chú admin.
    /// </summary>
    public string? AdminNote { get; init; }

    /// <summary>
    /// Định danh admin xử lý.
    /// </summary>
    public Guid AdminId { get; init; }

    /// <summary>
    /// Cờ xử lý thành công.
    /// </summary>
    public bool Processed { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
