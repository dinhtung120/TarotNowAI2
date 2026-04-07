using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Call.Queries.GetCallHistory;

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
        
        var pageSize = request.PageSize;
        if (pageSize > 50) pageSize = 50;
        if (pageSize < 1) pageSize = 20;
        
        var page = request.Page;
        if (page < 1) page = 1;

        
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        
        
        var participantStr = request.ParticipantId.ToString();
        if (conversation.UserId != participantStr && conversation.ReaderId != participantStr)
            throw new ForbiddenException("Bạn không phải thành viên, không được phép xem lịch sử truy cập này.");

        
        var result = await _callSessionRepository.GetByConversationIdPaginatedAsync(
            request.ConversationId, 
            page, 
            pageSize, 
            cancellationToken);

        return result;
    }
}
