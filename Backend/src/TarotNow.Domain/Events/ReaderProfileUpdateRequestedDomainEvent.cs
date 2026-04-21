namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu cập nhật hồ sơ Reader.
/// </summary>
public sealed class ReaderProfileUpdateRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh Reader.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Bio tiếng Việt.
    /// </summary>
    public string? BioVi { get; init; }

    /// <summary>
    /// Bio tiếng Anh.
    /// </summary>
    public string? BioEn { get; init; }

    /// <summary>
    /// Bio tiếng Trung.
    /// </summary>
    public string? BioZh { get; init; }

    /// <summary>
    /// Giá diamond mỗi câu hỏi.
    /// </summary>
    public long? DiamondPerQuestion { get; init; }

    /// <summary>
    /// Danh sách chuyên môn cập nhật.
    /// </summary>
    public IReadOnlyList<string>? Specialties { get; init; }

    /// <summary>
    /// Số năm kinh nghiệm cập nhật.
    /// </summary>
    public int? YearsOfExperience { get; init; }

    /// <summary>
    /// Link Facebook cập nhật.
    /// </summary>
    public string? FacebookUrl { get; init; }

    /// <summary>
    /// Link Instagram cập nhật.
    /// </summary>
    public string? InstagramUrl { get; init; }

    /// <summary>
    /// Link TikTok cập nhật.
    /// </summary>
    public string? TikTokUrl { get; init; }

    /// <summary>
    /// Cờ xử lý thành công.
    /// </summary>
    public bool Updated { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
