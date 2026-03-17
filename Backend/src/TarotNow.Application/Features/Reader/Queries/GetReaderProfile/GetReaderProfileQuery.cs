using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

/// <summary>
/// Query lấy thông tin hồ sơ Reader theo userId.
/// Trả về null nếu reader chưa có profile (chưa được approve).
/// </summary>
public class GetReaderProfileQuery : IRequest<ReaderProfileDto?>
{
    /// <summary>UUID của reader cần xem profile.</summary>
    public string UserId { get; set; } = string.Empty;
}
