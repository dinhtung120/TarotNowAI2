namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu cập nhật hồ sơ người dùng.
/// </summary>
public sealed class UserProfileUpdateRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng cần cập nhật.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Tên hiển thị mới.
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    /// Ngày sinh mới.
    /// </summary>
    public DateTime DateOfBirth { get; init; }

    /// <summary>
    /// Tên ngân hàng rút tiền.
    /// </summary>
    public string? PayoutBankName { get; init; }

    /// <summary>
    /// Mã BIN ngân hàng.
    /// </summary>
    public string? PayoutBankBin { get; init; }

    /// <summary>
    /// Số tài khoản nhận tiền.
    /// </summary>
    public string? PayoutBankAccountNumber { get; init; }

    /// <summary>
    /// Tên chủ tài khoản nhận tiền.
    /// </summary>
    public string? PayoutBankAccountHolder { get; init; }

    /// <summary>
    /// Kết quả xử lý cập nhật profile.
    /// </summary>
    public bool Updated { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
