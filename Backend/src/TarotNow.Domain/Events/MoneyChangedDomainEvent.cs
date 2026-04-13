namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi số dư ví người dùng thay đổi.
/// </summary>
public sealed class MoneyChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng có thay đổi số dư.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Loại tiền tệ thay đổi.
    /// </summary>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Loại biến động số dư.
    /// </summary>
    public string ChangeType { get; init; } = string.Empty;

    /// <summary>
    /// Giá trị thay đổi số dư (âm: trừ, dương: cộng).
    /// </summary>
    public long DeltaAmount { get; init; }

    /// <summary>
    /// Tham chiếu nghiệp vụ liên quan.
    /// </summary>
    public string? ReferenceId { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
