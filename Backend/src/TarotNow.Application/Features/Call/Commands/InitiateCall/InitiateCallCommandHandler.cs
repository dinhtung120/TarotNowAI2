using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

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
        
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện này.");

        
        if (conversation.Status != "ongoing")
            throw new BadRequestException("Chỉ có thể gọi khi cuộc trò chuyện đang diễn ra (ongoing).");

        
        var callerIdString = request.InitiatorId.ToString();
        if (conversation.UserId != callerIdString && conversation.ReaderId != callerIdString)
            throw new ForbiddenException("Bạn không phải thành viên của cuộc trò chuyện này nên không thế bắt đầu cuộc gọi.");

        
        var existingCall = await _callSessionRepository.GetActiveByConversationAsync(request.ConversationId, cancellationToken);
        if (existingCall != null)
            throw new BadRequestException("Đang có một cuộc gọi khác diễn ra trong trò chuyện này.");

        
        var callType = ParseCallTypeOrThrow(request.Type);
        var newCallSession = new CallSessionDto
        {
            ConversationId = request.ConversationId,
            InitiatorId = callerIdString,
            Type = callType,
            Status = CallSessionStatus.Requested,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        
        await _callSessionRepository.AddAsync(newCallSession, cancellationToken);

        return newCallSession;
    }

    private static CallType ParseCallTypeOrThrow(string rawType)
    {
        if (string.IsNullOrWhiteSpace(rawType))
        {
            throw new BadRequestException("Loại cuộc gọi không hợp lệ.");
        }

        return rawType.Trim().ToLowerInvariant() switch
        {
            "audio" => CallType.Audio,
            "video" => CallType.Video,
            _ => throw new BadRequestException("Loại cuộc gọi không hợp lệ.")
        };
    }
}
