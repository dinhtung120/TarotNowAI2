using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

/// <summary>
/// Command cho Admin phê duyệt hoặc từ chối đơn xin Reader.
///
/// Luồng nghiệp vụ:
/// - Approve: cập nhật user.role → tarot_reader, tạo reader_profiles, audit trail.
/// - Reject: cập nhật reader_requests status → rejected, giữ user.role = user.
///
/// RequestId là ObjectId string từ MongoDB reader_requests collection.
/// </summary>
public class ApproveReaderCommand : IRequest<bool>
{
    /// <summary>ObjectId string của reader_requests document.</summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Hành động: "approve" | "reject".
    /// Validate trong Handler.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Ghi chú của admin — giải thích quyết định (tùy chọn).</summary>
    public string? AdminNote { get; set; }

    /// <summary>UUID admin thực hiện — lấy từ JWT claims trong Controller.</summary>
    public Guid AdminId { get; set; }
}
