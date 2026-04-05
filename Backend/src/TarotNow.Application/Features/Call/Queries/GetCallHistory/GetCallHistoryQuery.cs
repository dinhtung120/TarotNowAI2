using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

/// <summary>
/// Query lấy danh sách lịch sử cuộc gọi trong một cuộc trò chuyện.
/// Trả về số lượng item của page đó và cả tổng số lượng record thoả mãn.
/// </summary>
public class GetCallHistoryQuery : IRequest<(IEnumerable<CallSessionDto> Items, long TotalCount)>
{
    /// <summary>
    /// ID của Conversation muốn tải lịch sử cuộc gọi.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của tài khoản làm query (để kiểm tra xem có quyền truy cập không).
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Số thứ tự trang.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Giới hạn số phần tử trong trang.
    /// </summary>
    public int PageSize { get; set; } = 20;
}
