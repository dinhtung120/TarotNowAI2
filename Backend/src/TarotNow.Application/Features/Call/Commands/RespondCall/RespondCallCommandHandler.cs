using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

public class RespondCallCommandHandler : IRequestHandler<RespondCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    public RespondCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<CallSessionDto> Handle(RespondCallCommand request, CancellationToken cancellationToken)
    {
        
        var session = await _callSessionRepository.GetByIdAsync(request.CallSessionId, cancellationToken);
        if (session == null)
            throw new NotFoundException("Không tìm thấy cuộc gọi này.");

        
        if (session.Status != CallSessionStatus.Requested)
            throw new BadRequestException("Không thể phản hồi cuộc gọi vì cuộc gọi không ở trạng thái đang chờ.");

        
        var responderIdStr = request.ResponderId.ToString();
        var conversation = await _conversationRepository.GetByIdAsync(session.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện của cuộc gọi này.");

        
        if (conversation.UserId != responderIdStr && conversation.ReaderId != responderIdStr)
            throw new ForbiddenException("Bạn không phải là thành viên trong cuộc trò chuyện này.");
            
        if (session.InitiatorId == responderIdStr)
            throw new BadRequestException("Người khởi tạo không thế tự phản hồi (Accept/Reject) chính cuộc gọi do mình phát bắt đầu.");

        
        var newStatus = request.Accept ? CallSessionStatus.Accepted : CallSessionStatus.Rejected;
        DateTime? startedAt = request.Accept ? DateTime.UtcNow : null;

        
        var updated = await _callSessionRepository.UpdateStatusAsync(
            session.Id,
            newStatus,
            startedAt: startedAt,
            endedAt: null,
            endReason: null,
            expectedPreviousStatus: CallSessionStatus.Requested,
            ct: cancellationToken);

        if (!updated)
            throw new BadRequestException("Không thể phản hồi cuộc gọi vì nó đã bị hủy hoặc đối phương đã đóng kết nối.");

        
        
        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }
}
