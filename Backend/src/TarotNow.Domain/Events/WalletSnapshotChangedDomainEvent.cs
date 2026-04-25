namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi snapshot ví thay đổi nhưng không làm đổi số dư khả dụng.
/// </summary>
public sealed class WalletSnapshotChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh user có thay đổi snapshot ví.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Loại tiền tệ liên quan.
    /// </summary>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Loại thay đổi nghiệp vụ (escrow/withdrawal...).
    /// </summary>
    public string ChangeType { get; init; } = string.Empty;

    /// <summary>
    /// Tham chiếu nghiệp vụ.
    /// </summary>
    public string? ReferenceId { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
