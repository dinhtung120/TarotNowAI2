using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

// Handler kết thúc phiên gọi theo quyền participant và trạng thái hiện tại.
public class EndCallCommandHandler : IRequestHandler<EndCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler end call.
    /// Luồng xử lý: nhận call session repository và conversation repository để kiểm tra quyền trước khi đổi trạng thái.
    /// </summary>
    public EndCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý command kết thúc cuộc gọi.
    /// Luồng xử lý: tải session, kiểm tra trạng thái và quyền user, cập nhật trạng thái ended theo optimistic check.
    /// </summary>
    public async Task<CallSessionDto> Handle(EndCallCommand request, CancellationToken cancellationToken)
    {
        var session = await GetSessionOrThrowAsync(request.CallSessionId, cancellationToken);
        EnsureSessionCanBeEnded(session);
        await EnsureUserCanEndCallAsync(session, request.UserId, request.Reason, cancellationToken);

        var endedAt = DateTime.UtcNow;
        var updated = await _callSessionRepository.UpdateStatusAsync(
            session.Id,
            CallSessionStatus.Ended,
            startedAt: null,
            endedAt: endedAt,
            endReason: request.Reason,
            expectedPreviousStatus: session.Status,
            ct: cancellationToken);

        if (!updated)
        {
            // Nhánh race condition: trạng thái đã bị cập nhật bởi request khác trước đó.
            throw new BadRequestException("Không thể kết thúc cuộc gọi này do trạng thái đã bị thay đổi (có thể đã kết thúc).");
        }

        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }

    /// <summary>
    /// Tải call session theo id hoặc ném NotFoundException.
    /// Luồng xử lý: gọi repository theo id và fail-fast nếu không tồn tại.
    /// </summary>
    private async Task<CallSessionDto> GetSessionOrThrowAsync(string callSessionId, CancellationToken cancellationToken)
    {
        return await _callSessionRepository.GetByIdAsync(callSessionId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc gọi này.");
    }

    /// <summary>
    /// Đảm bảo session hiện tại vẫn có thể kết thúc.
    /// Luồng xử lý: chặn các session đã ended/rejected.
    /// </summary>
    private static void EnsureSessionCanBeEnded(CallSessionDto session)
    {
        if (session.Status is CallSessionStatus.Ended or CallSessionStatus.Rejected)
        {
            throw new BadRequestException("Cuộc gọi này đã kết thúc trước đó.");
        }
    }

    /// <summary>
    /// Kiểm tra quyền của user đối với thao tác kết thúc cuộc gọi.
    /// Luồng xử lý: xác nhận user là participant, xử lý rule đặc biệt cho trạng thái Requested.
    /// </summary>
    private async Task EnsureUserCanEndCallAsync(
        CallSessionDto session,
        Guid userId,
        string reason,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(session.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện của cuộc gọi này.");

        var userIdText = userId.ToString();
        if (!IsConversationParticipant(conversation, userIdText))
        {
            // Chặn user ngoài cuộc hội thoại thao tác call.
            throw new ForbiddenException("Bạn không phải thành viên, không thể huỷ/kết thúc cuộc gọi.");
        }

        var canCancelRequested = session.Status != CallSessionStatus.Requested
            || session.InitiatorId == userIdText
            || string.Equals(reason, "timeout", StringComparison.OrdinalIgnoreCase);
        if (!canCancelRequested)
        {
            // Rule nghiệp vụ: người không khởi tạo không được cancel requested call trừ timeout.
            throw new BadRequestException("Bạn không phải người gọi, hãy dùng tính năng Từ Chối thay vì Huỷ cuộc gọi.");
        }
    }

    /// <summary>
    /// Kiểm tra user có thuộc hai đầu participant của conversation hay không.
    /// Luồng xử lý: so sánh user id với user/reader id của conversation.
    /// </summary>
    private static bool IsConversationParticipant(ConversationDto conversation, string userId)
    {
        return conversation.UserId == userId || conversation.ReaderId == userId;
    }
}
