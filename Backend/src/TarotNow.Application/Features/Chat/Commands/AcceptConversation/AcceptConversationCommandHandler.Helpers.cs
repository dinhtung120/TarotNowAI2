using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.AcceptConversation;

public partial class AcceptConversationCommandHandler
{
    private async Task<DateTime> AddAcceptedSystemMessageAsync(
        ConversationDto conversation,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var systemMessage = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.System,
            Content = "Reader đã chấp nhận câu hỏi. Cuộc trò chuyện bắt đầu.",
            IsRead = false,
            CreatedAt = now
        };

        await _chatMessageRepository.AddAsync(systemMessage, cancellationToken);
        return systemMessage.CreatedAt;
    }

    private static int ResolveSlaHours(int configuredSlaHours)
    {
        return configuredSlaHours is 6 or 12 or 24 ? configuredSlaHours : 12;
    }
}
