using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
{
    private readonly record struct SystemMessageSpec(string Type, string Content, DateTime? CreatedAt);

    private async Task<DateTime> AddSystemMessageAsync(
        ConversationDto conversation,
        string senderId,
        SystemMessageSpec spec,
        CancellationToken cancellationToken)
    {
        var message = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = senderId,
            Type = spec.Type,
            Content = spec.Content,
            IsRead = false,
            CreatedAt = spec.CreatedAt ?? DateTime.UtcNow
        };

        await _chatMessageRepository.AddAsync(message, cancellationToken);
        return message.CreatedAt;
    }
}
