using MediatR;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

/// <summary>
/// Command cập nhật hồ sơ Reader.
/// </summary>
public class UpdateReaderProfileCommand : IRequest<bool>
{
    /// <summary>
    /// Định danh user của Reader.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Bio tiếng Việt.
    /// </summary>
    public string? BioVi { get; set; }

    /// <summary>
    /// Bio tiếng Anh.
    /// </summary>
    public string? BioEn { get; set; }

    /// <summary>
    /// Bio tiếng Trung.
    /// </summary>
    public string? BioZh { get; set; }

    /// <summary>
    /// Giá diamond mỗi câu hỏi.
    /// </summary>
    public long? DiamondPerQuestion { get; set; }

    /// <summary>
    /// Danh sách chuyên môn.
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
