using MediatR;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Command gửi đơn đăng ký trở thành Reader.
/// </summary>
public class SubmitReaderRequestCommand : IRequest<bool>
{
    /// <summary>
    /// Định danh user gửi đơn.
    /// </summary>
    public Guid UserId { get; set; }

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
    /// Danh sách tài liệu minh chứng.
    /// </summary>
    public List<string> ProofDocuments { get; set; } = [];
}
