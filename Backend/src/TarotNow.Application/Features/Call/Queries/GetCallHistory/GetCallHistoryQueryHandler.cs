using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

// Handler truy vấn lịch sử cuộc gọi theo conversation.
public class GetCallHistoryQueryHandler : IRequestHandler<GetCallHistoryQuery, (IEnumerable<CallSessionDto> Items, long TotalCount)>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler get call history.
    /// Luồng xử lý: nhận call session repository và conversation repository để kiểm tra quyền + truy vấn dữ liệu.
    /// </summary>
    public GetCallHistoryQueryHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý query lấy lịch sử cuộc gọi.
    /// Luồng xử lý: chuẩn hóa page/pageSize, kiểm tra conversation tồn tại và quyền participant, truy vấn paginated history.
    /// </summary>
    public async Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> Handle(GetCallHistoryQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        if (pageSize > 50)
        {
            // Giới hạn cứng page size để bảo vệ hiệu năng truy vấn.
            pageSize = 50;
        }

        if (pageSize < 1)
        {
            // Edge case input page size không hợp lệ: fallback về giá trị mặc định.
            pageSize = 20;
        }

        var page = request.Page;
        if (page < 1)
        {
            // Edge case page âm/0: fallback về trang đầu tiên.
            page = 1;
        }

        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
        {
            // Conversation không tồn tại.
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
        }

        var participantStr = request.ParticipantId.ToString();
        if (conversation.UserId != participantStr && conversation.ReaderId != participantStr)
        {
            // Chặn truy cập lịch sử cuộc gọi từ người không thuộc conversation.
            throw new ForbiddenException("Bạn không phải thành viên, không được phép xem lịch sử truy cập này.");
        }

        var result = await _callSessionRepository.GetByConversationIdPaginatedAsync(
            request.ConversationId,
            page,
            pageSize,
            cancellationToken);

        return result;
    }
}
