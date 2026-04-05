using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

/// <summary>
/// Handler xử lý logic đồng ý hoặc từ chối cuộc gọi đến.
/// </summary>
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
        // 1. Kiểm tra session cuộc gọi
        var session = await _callSessionRepository.GetByIdAsync(request.CallSessionId, cancellationToken);
        if (session == null)
            throw new NotFoundException("Không tìm thấy cuộc gọi này.");

        // 2. Phải là trạng thái đang yêu cầu (Requested) mới được phản hồi
        if (session.Status != CallSessionStatus.Requested)
            throw new BadRequestException("Không thể phản hồi cuộc gọi vì cuộc gọi không ở trạng thái đang chờ.");

        // 3. Kiểm tra tính hợp lệ của người phản hồi
        var responderIdStr = request.ResponderId.ToString();
        var conversation = await _conversationRepository.GetByIdAsync(session.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện của cuộc gọi này.");

        // Người phản hồi phải là thành viên trong conversation và KHÔNG PHẢI là người khởi tạo cuộc gọi.
        if (conversation.UserId != responderIdStr && conversation.ReaderId != responderIdStr)
            throw new ForbiddenException("Bạn không phải là thành viên trong cuộc trò chuyện này.");
            
        if (session.InitiatorId == responderIdStr)
            throw new BadRequestException("Người khởi tạo không thế tự phản hồi (Accept/Reject) chính cuộc gọi do mình phát bắt đầu.");

        // 4. Xử lý Accept hay Reject
        var newStatus = request.Accept ? CallSessionStatus.Accepted : CallSessionStatus.Rejected;
        DateTime? startedAt = request.Accept ? DateTime.UtcNow : null;

        // Cập nhật Atomic (tức thì và an toàn) với ExpectedPreviousStatus
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

        // Nạp data mới nhất (atomic update không trả về kết quả mới bên phía abstraction hiện tại)
        // Việc update status và nạp lại đảm bảo ta trả dữ liệu đúng nhât sau request (CQRS)
        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }
}
