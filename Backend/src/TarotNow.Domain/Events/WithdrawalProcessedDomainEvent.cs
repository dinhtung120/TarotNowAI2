namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi admin đã xử lý xong withdrawal request.
/// </summary>
public sealed class WithdrawalProcessedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh yêu cầu rút tiền.
    /// </summary>
    public Guid RequestId { get; init; }

    /// <summary>
    /// Định danh user của yêu cầu.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Định danh admin xử lý.
    /// </summary>
    public Guid AdminId { get; init; }

    /// <summary>
    /// Action đã áp dụng.
    /// </summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>
    /// Trạng thái request sau xử lý.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Số diamond của yêu cầu.
    /// </summary>
    public long AmountDiamond { get; init; }

    /// <summary>
    /// Ghi chú admin.
    /// </summary>
    public string? AdminNote { get; init; }

    /// <summary>
    /// Thời điểm xử lý.
    /// </summary>
    public DateTime ProcessedAtUtc { get; init; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
