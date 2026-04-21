namespace TarotNow.Api.Contracts.Requests;

/// <summary>
/// Payload người dùng gửi hồ sơ đăng ký trở thành Reader.
/// </summary>
public class SubmitReaderRequestBody
{
    /// <summary>
    /// Lời giới thiệu Reader.
    /// </summary>
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn đã chọn.
    /// </summary>
    public List<string> Specialties { get; set; } = [];

    /// <summary>
    /// Số năm kinh nghiệm.
    /// </summary>
    public int YearsOfExperience { get; set; }

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

    /// <summary>
    /// Giá Diamond mỗi câu hỏi.
    /// </summary>
    public long DiamondPerQuestion { get; set; }

    /// <summary>
    /// Danh sách tài liệu minh chứng đính kèm.
    /// </summary>
    public List<string>? ProofDocuments { get; set; }
}
