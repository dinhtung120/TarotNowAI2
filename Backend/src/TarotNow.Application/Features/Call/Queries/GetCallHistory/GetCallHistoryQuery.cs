using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

// Query lấy lịch sử cuộc gọi theo conversation và participant.
public class GetCallHistoryQuery : IRequest<(IEnumerable<CallSessionDto> Items, long TotalCount)>
{
    // Định danh conversation cần xem lịch sử gọi.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh participant yêu cầu truy vấn (dùng kiểm tra quyền).
    public Guid ParticipantId { get; set; }

    // Trang hiện tại (1-based).
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;
}
