using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

/// <summary>
/// Handler xử lý logic việc tạo (Initiate) một cuộc gọi mới.
/// </summary>
public class InitiateCallCommandHandler : IRequestHandler<InitiateCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    public InitiateCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<CallSessionDto> Handle(InitiateCallCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra conversation có tồn tại không
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện này.");

        // 2. Validate trạng thái cuộc trò chuyện (phải là ongoing)
        if (conversation.Status != "ongoing")
            throw new BadRequestException("Chỉ có thể gọi khi cuộc trò chuyện đang diễn ra (ongoing).");

        // 3. Guard participant (chỉ user hoặc reader trong conversation mới được quyền gọi)
        var callerIdString = request.InitiatorId.ToString();
        if (conversation.UserId != callerIdString && conversation.ReaderId != callerIdString)
            throw new ForbiddenException("Bạn không phải thành viên của cuộc trò chuyện này nên không thế bắt đầu cuộc gọi.");

        // 4. Guard active call: Mỗi conversation chỉ được có 1 cuộc gọi active (Requested hoặc Accepted) cùng lúc
        var existingCall = await _callSessionRepository.GetActiveByConversationAsync(request.ConversationId, cancellationToken);
        if (existingCall != null)
            throw new BadRequestException("Đang có một cuộc gọi khác diễn ra trong trò chuyện này.");

        // 5. Khởi tạo phiên gọi (status là Requested)
        var newCallSession = new CallSessionDto
        {
            ConversationId = request.ConversationId,
            InitiatorId = callerIdString,
            Type = request.Type,
            Status = CallSessionStatus.Requested,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 6. Lưu xuống database thông qua repository
        await _callSessionRepository.AddAsync(newCallSession, cancellationToken);

        return newCallSession;
    }
}
