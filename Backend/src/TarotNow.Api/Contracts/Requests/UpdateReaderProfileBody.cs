namespace TarotNow.Api.Contracts.Requests;

/// <summary>
/// Payload cập nhật hồ sơ công khai của Reader.
/// </summary>
public class UpdateReaderProfileBody
{
    /// <summary>
    /// Mô tả profile bằng tiếng Việt.
    /// </summary>
    public string? BioVi { get; set; }

    /// <summary>
    /// Mô tả profile bằng tiếng Anh.
    /// </summary>
    public string? BioEn { get; set; }

    /// <summary>
    /// Mô tả profile bằng tiếng Trung.
    /// </summary>
    public string? BioZh { get; set; }

    /// <summary>
    /// Giá theo kim cương cho mỗi câu hỏi Reader nhận trả lời.
    /// </summary>
    public long? DiamondPerQuestion { get; set; }

    /// <summary>
    /// Danh sách chuyên môn Reader.
    /// </summary>
    public List<string>? Specialties { get; set; }

    /// <summary>
    /// Số năm kinh nghiệm.
    /// </summary>
    public int? YearsOfExperience { get; set; }

    /// <summary>
    /// Link Facebook.
    /// </summary>
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Link Instagram.
    /// </summary>
    public string? InstagramUrl { get; set; }

    /// <summary>
    /// Link TikTok.
    /// </summary>
    public string? TikTokUrl { get; set; }
}
