namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu cập nhật trạng thái hoạt động Reader.
/// </summary>
public sealed class ReaderStatusUpdateRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh Reader.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Trạng thái yêu cầu cập nhật.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Cờ cập nhật thành công.
    /// </summary>
    public bool Updated { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
