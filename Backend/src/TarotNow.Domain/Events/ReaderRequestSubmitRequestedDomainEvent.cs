namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu gửi đơn đăng ký Reader.
/// </summary>
public sealed class ReaderRequestSubmitRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng gửi đơn.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Lời giới thiệu Reader.
    /// </summary>
    public string Bio { get; init; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn.
    /// </summary>
    public IReadOnlyList<string> Specialties { get; init; } = [];

    /// <summary>
    /// Số năm kinh nghiệm.
    /// </summary>
    public int YearsOfExperience { get; init; }

    /// <summary>
    /// Link Facebook.
    /// </summary>
    public string? FacebookUrl { get; init; }

    /// <summary>
    /// Link Instagram.
    /// </summary>
    public string? InstagramUrl { get; init; }

    /// <summary>
    /// Link TikTok.
    /// </summary>
    public string? TikTokUrl { get; init; }

    /// <summary>
    /// Giá diamond mỗi câu hỏi.
    /// </summary>
    public long DiamondPerQuestion { get; init; }

    /// <summary>
    /// Danh sách tài liệu đính kèm.
    /// </summary>
    public IReadOnlyList<string> ProofDocuments { get; init; } = [];

    /// <summary>
    /// Cờ xử lý thành công.
    /// </summary>
    public bool Submitted { get; set; }

    /// <summary>
    /// Định danh đơn vừa tạo.
    /// </summary>
    public string? RequestId { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
