using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

// Handler khởi tạo cuộc gọi mới trong conversation.
public class InitiateCallCommandHandler : IRequestHandler<InitiateCallCommand, CallSessionDto>
{
    private readonly ICallSessionRepository _callSessionRepository;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler initiate call.
    /// Luồng xử lý: nhận call session repository và conversation repository để kiểm tra điều kiện trước khi tạo call.
    /// </summary>
    public InitiateCallCommandHandler(
        ICallSessionRepository callSessionRepository,
        IConversationRepository conversationRepository)
    {
        _callSessionRepository = callSessionRepository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý command khởi tạo cuộc gọi.
    /// Luồng xử lý: kiểm tra conversation tồn tại/ongoing, kiểm tra participant, chặn active call trùng, tạo session requested.
    /// </summary>
    public async Task<CallSessionDto> Handle(InitiateCallCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
        {
            // Conversation không tồn tại thì không thể khởi tạo cuộc gọi.
            throw new NotFoundException("Không tìm thấy cuộc trò chuyện này.");
        }

        if (conversation.Status != "ongoing")
        {
            // Rule nghiệp vụ: chỉ cho phép gọi khi conversation đang ongoing.
            throw new BadRequestException("Chỉ có thể gọi khi cuộc trò chuyện đang diễn ra (ongoing).");
        }

        var callerIdString = request.InitiatorId.ToString();
        if (conversation.UserId != callerIdString && conversation.ReaderId != callerIdString)
        {
            // Chặn user ngoài conversation tạo cuộc gọi.
            throw new ForbiddenException("Bạn không phải thành viên của cuộc trò chuyện này nên không thế bắt đầu cuộc gọi.");
        }

        var existingCall = await _callSessionRepository.GetActiveByConversationAsync(request.ConversationId, cancellationToken);
        if (existingCall != null)
        {
            // Edge case đã có active call: không tạo session mới để tránh xung đột signaling.
            throw new BadRequestException("Đang có một cuộc gọi khác diễn ra trong trò chuyện này.");
        }

        // Parse type về enum domain trước khi tạo session.
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

        // Persist session mới ở trạng thái Requested.
        await _callSessionRepository.AddAsync(newCallSession, cancellationToken);

        return newCallSession;
    }

    /// <summary>
    /// Parse loại cuộc gọi từ chuỗi đầu vào sang enum.
    /// Luồng xử lý: trim+lower input, map audio/video, ném BadRequest cho giá trị khác.
    /// </summary>
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
