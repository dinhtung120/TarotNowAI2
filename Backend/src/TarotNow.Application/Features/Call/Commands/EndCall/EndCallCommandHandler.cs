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
        var session = await GetSessionOrThrowAsync(request.CallSessionId, cancellationToken);
        EnsureSessionCanBeEnded(session);
        await EnsureUserCanEndCallAsync(session, request.UserId, request.Reason, cancellationToken);

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
        {
            throw new BadRequestException("Không thể kết thúc cuộc gọi này do trạng thái đã bị thay đổi (có thể đã kết thúc).");
        }

        var updatedSession = await _callSessionRepository.GetByIdAsync(session.Id, cancellationToken);
        return updatedSession ?? session;
    }

    private async Task<CallSessionDto> GetSessionOrThrowAsync(string callSessionId, CancellationToken cancellationToken)
    {
        return await _callSessionRepository.GetByIdAsync(callSessionId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc gọi này.");
    }

    private static void EnsureSessionCanBeEnded(CallSessionDto session)
    {
        if (session.Status is CallSessionStatus.Ended or CallSessionStatus.Rejected)
        {
            throw new BadRequestException("Cuộc gọi này đã kết thúc trước đó.");
        }
    }

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
            throw new ForbiddenException("Bạn không phải thành viên, không thể huỷ/kết thúc cuộc gọi.");
        }

        var canCancelRequested = session.Status != CallSessionStatus.Requested
            || session.InitiatorId == userIdText
            || string.Equals(reason, "timeout", StringComparison.OrdinalIgnoreCase);
        if (!canCancelRequested)
        {
            throw new BadRequestException("Bạn không phải người gọi, hãy dùng tính năng Từ Chối thay vì Huỷ cuộc gọi.");
        }
    }

    private static bool IsConversationParticipant(ConversationDto conversation, string userId)
    {
        return conversation.UserId == userId || conversation.ReaderId == userId;
    }
}
