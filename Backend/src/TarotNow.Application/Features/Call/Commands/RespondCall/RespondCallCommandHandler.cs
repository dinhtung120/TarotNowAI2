using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

// Handler xử lý thao tác accept/reject cho cuộc gọi đang chờ.
public class RespondCallCommandHandler : IRequestHandler<RespondCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler respond call.
    /// Luồng xử lý: nhận call session repository và conversation repository để kiểm tra quyền và cập nhật trạng thái.
    /// </summary>
    public RespondCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý command phản hồi cuộc gọi.
    /// Luồng xử lý: tải session requested, kiểm tra participant, chặn initiator tự phản hồi, cập nhật trạng thái accept/reject.
    /// </summary>
    public async Task<CallSessionDto> Handle(RespondCallCommand request, CancellationToken cancellationToken)
    {
        var session = await _callSessionRepository.GetByIdAsync(request.CallSessionId, cancellationToken);
        if (session == null)
        {
            // Session không tồn tại.
            throw new NotFoundException("Không tìm thấy cuộc gọi này.");
        }

        if (session.Status != CallSessionStatus.Requested)
        {
            // Chỉ phản hồi được cuộc gọi đang chờ.
            throw new BadRequestException("Không thể phản hồi cuộc gọi vì cuộc gọi không ở trạng thái đang chờ.");
        }

        var responderIdStr = request.ResponderId.ToString();
        var conversation = await _conversationRepository.GetByIdAsync(session.ConversationId, cancellationToken);
        if (conversation == null)
        {
            // Conversation gốc không tồn tại.
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện của cuộc gọi này.");
        }

        if (conversation.UserId != responderIdStr && conversation.ReaderId != responderIdStr)
        {
            // Chặn user không thuộc conversation phản hồi cuộc gọi.
            throw new ForbiddenException("Bạn không phải là thành viên trong cuộc trò chuyện này.");
        }

        if (session.InitiatorId == responderIdStr)
        {
            // Rule nghiệp vụ: initiator không được tự accept/reject cuộc gọi do mình tạo.
            throw new BadRequestException("Người khởi tạo không thế tự phản hồi (Accept/Reject) chính cuộc gọi do mình phát bắt đầu.");
        }

        var newStatus = request.Accept ? CallSessionStatus.Accepted : CallSessionStatus.Rejected;
        DateTime? startedAt = request.Accept ? DateTime.UtcNow : null;

        // Dùng expectedPreviousStatus để chống race condition khi nhiều client phản hồi cùng lúc.
        var updated = await _callSessionRepository.UpdateStatusAsync(
            session.Id,
            newStatus,
            startedAt: startedAt,
            endedAt: null,
            endReason: null,
            expectedPreviousStatus: CallSessionStatus.Requested,
            ct: cancellationToken);

        if (!updated)
        {
            // Nhánh conflict trạng thái do cuộc gọi bị cancel/đóng trước khi phản hồi.
            throw new BadRequestException("Không thể phản hồi cuộc gọi vì nó đã bị hủy hoặc đối phương đã đóng kết nối.");
        }

        // Trả session mới nhất sau khi cập nhật thành công.
        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }
}
