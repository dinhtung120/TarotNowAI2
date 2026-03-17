using MediatR;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

/// <summary>
/// Command cập nhật hồ sơ công khai Reader (bio, pricing, specialties).
///
/// Chỉ reader sở hữu profile mới được update — check ownership trong Handler.
/// UserId lấy từ JWT claims, không từ request body.
/// </summary>
public class UpdateReaderProfileCommand : IRequest<bool>
{
    /// <summary>UUID reader — lấy từ JWT claims.</summary>
    public Guid UserId { get; set; }

    /// <summary>Mô tả bản thân tiếng Việt.</summary>
    public string? BioVi { get; set; }

    /// <summary>Mô tả bản thân tiếng Anh.</summary>
    public string? BioEn { get; set; }

    /// <summary>Mô tả bản thân tiếng Trung.</summary>
    public string? BioZh { get; set; }

    /// <summary>Giá Diamond mỗi câu hỏi.</summary>
    public long? DiamondPerQuestion { get; set; }

    /// <summary>Cập nhật danh sách chuyên môn.</summary>
    public List<string>? Specialties { get; set; }
}
