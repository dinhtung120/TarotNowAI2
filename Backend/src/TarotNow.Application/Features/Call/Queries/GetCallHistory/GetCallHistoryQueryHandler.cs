using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

/// <summary>
/// Handler xử lý query xem lịch sử cuộc gọi theo phần trang.
/// Trả về paginated list mới nhất trước.
/// </summary>
public class GetCallHistoryQueryHandler : IRequestHandler<GetCallHistoryQuery, (IEnumerable<CallSessionDto> Items, long TotalCount)>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    public GetCallHistoryQueryHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> Handle(GetCallHistoryQuery request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra giới hạn pageSize chặn lặp quá tải 
        var pageSize = request.PageSize;
        if (pageSize > 50) pageSize = 50;
        if (pageSize < 1) pageSize = 20;
        
        var page = request.Page;
        if (page < 1) page = 1;

        // 2. Load conversation
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // 3. Guard participant:
        // Cả khi conversation completed, người dùng vẫn có thể xem được cái lịch sử call của họ.
        var participantStr = request.ParticipantId.ToString();
        if (conversation.UserId != participantStr && conversation.ReaderId != participantStr)
            throw new ForbiddenException("Bạn không phải thành viên, không được phép xem lịch sử truy cập này.");

        // 4. Lấy qua DB
        var result = await _callSessionRepository.GetByConversationIdPaginatedAsync(
            request.ConversationId, 
            page, 
            pageSize, 
            cancellationToken);

        return result;
    }
}
