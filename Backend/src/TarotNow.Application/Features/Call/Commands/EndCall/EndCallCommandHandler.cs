using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

/// <summary>
/// Handler xử lý kết thúc một cuộc gọi đang diễn ra hoặc đang chờ.
/// </summary>
public class EndCallCommandHandler : IRequestHandler<EndCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    public EndCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<CallSessionDto> Handle(EndCallCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra session cuộc gọi
        var session = await _callSessionRepository.GetByIdAsync(request.CallSessionId, cancellationToken);
        if (session == null)
            throw new NotFoundException("Không tìm thấy cuộc gọi này.");

        // Chỉ xử lý end khi Call đang diễn ra (Accepted) hoặc lúc người gọi đang chờ (Requested) và muốn Cancel/Timeout.
        // Ngược lại, báo lỗi không hợp lệ nếu call tự nó đã end.
        if (session.Status == CallSessionStatus.Ended || session.Status == CallSessionStatus.Rejected)
            throw new BadRequestException("Cuộc gọi này đã kết thúc trước đó.");

        // 2. Guard participant quyền kết thúc (Phải là người thuộc conversation đó)
        var conversation = await _conversationRepository.GetByIdAsync(session.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện của cuộc gọi này.");

        var userIdStr = request.UserId.ToString();
        if (conversation.UserId != userIdStr && conversation.ReaderId != userIdStr)
            throw new ForbiddenException("Bạn không phải thành viên, không thể huỷ/kết thúc cuộc gọi.");

        // Nếu cuộc gọi đang Requested mà người nghe lại nhấn "Cancel/End" sẽ gọi API gì?
        // (Về nguyên tắc, nếu Requested: 
        //   - caller huỷ gọi = cancel (vào file này)
        //   - callee từ chối = reject (vào luồng RespondCall)
        if (session.Status == CallSessionStatus.Requested && session.InitiatorId != userIdStr && request.Reason != "timeout")
            throw new BadRequestException("Bạn không phải người gọi, hãy dùng tính năng Từ Chối thay vì Huỷ cuộc gọi.");

        // 3. Thực hiện update Atomic thành Ended
        var endedAt = DateTime.UtcNow;

        var updated = await _callSessionRepository.UpdateStatusAsync(
            session.Id,
            CallSessionStatus.Ended,
            startedAt: null, // Không can thiệp ghi đè field này
            endedAt: endedAt,
            endReason: request.Reason,
            expectedPreviousStatus: session.Status, // FIX #17: Guard status
            ct: cancellationToken);

        if (!updated)
            throw new BadRequestException("Không thể kết thúc cuộc gọi này do trạng thái đã bị thay đổi (có thể đã kết thúc).");

        // Fetch mới DTO đảm bảo chính xác CQRS (bao gồm DB computed logic như duration nếu có).
        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }
}
