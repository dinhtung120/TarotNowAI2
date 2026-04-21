namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi yêu cầu rút tiền được tạo thành công.
/// </summary>
public sealed class WithdrawalRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh yêu cầu rút tiền.
    /// </summary>
    public Guid RequestId { get; init; }

    /// <summary>
    /// Định danh user tạo yêu cầu.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Số diamond yêu cầu rút.
    /// </summary>
    public long AmountDiamond { get; init; }

    /// <summary>
    /// Số tiền thực nhận VND.
    /// </summary>
    public long NetAmountVnd { get; init; }

    /// <summary>
    /// Tên ngân hàng nhận tiền.
    /// </summary>
    public string BankName { get; init; } = string.Empty;

    /// <summary>
    /// Số tài khoản ngân hàng nhận tiền.
    /// </summary>
    public string BankAccountNumber { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
