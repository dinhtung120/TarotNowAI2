namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi hệ thống phát hành OTP qua email.
/// </summary>
public sealed class EmailOtpIssuedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng nhận OTP.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email nhận OTP.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Chủ đề email.
    /// </summary>
    public string Subject { get; init; } = string.Empty;

    /// <summary>
    /// Nội dung email.
    /// </summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>
    /// Loại nghiệp vụ OTP.
    /// </summary>
    public string Purpose { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
