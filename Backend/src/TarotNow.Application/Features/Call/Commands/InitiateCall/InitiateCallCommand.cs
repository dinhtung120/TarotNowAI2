using MediatR;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

/// <summary>
/// Command dùng để khởi tạo một cuộc gọi (Audio/Video).
/// </summary>
public class InitiateCallCommand : IRequest<CallSessionDto>
{
    /// <summary>
    /// ID của cuộc trò chuyện (Conversation) diễn ra cuộc gọi này.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// UUID của người bấm nút "Gọi".
    /// </summary>
    public Guid InitiatorId { get; set; }

    /// <summary>
    /// Loại cuộc gọi (Audio hoặc Video).
    /// </summary>
    public CallType Type { get; set; }
}
