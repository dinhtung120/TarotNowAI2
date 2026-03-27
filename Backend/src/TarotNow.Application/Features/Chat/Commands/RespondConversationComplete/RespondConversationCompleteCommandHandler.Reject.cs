using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
{
    private async Task<ConversationCompleteRespondResult> RejectCompletionRequestAsync(
        ResponseContext context,
        CancellationToken cancellationToken)
    {
        var rejectMessage = BuildRejectMessage(context.Conversation);
        var rejectAt = await AddSystemMessageAsync(
            context.Conversation,
            context.RequesterId,
            new SystemMessageSpec(ChatMessageType.System, rejectMessage, DateTime.UtcNow),
            cancellationToken);

        context.Conversation.Confirm = null;
        context.Conversation.UpdatedAt = rejectAt;
        context.Conversation.LastMessageAt = rejectAt;
        await _conversationRepository.UpdateAsync(context.Conversation, cancellationToken);

        return new ConversationCompleteRespondResult
        {
            Status = context.Conversation.Status,
            Accepted = false
        };
    }

    private static string BuildRejectMessage(ConversationDto conversation)
    {
        return conversation.Confirm?.RequestedBy == conversation.ReaderId
            ? "User đã từ chối yêu cầu hoàn thành của Reader. Bạn có thể mở tranh chấp."
            : "Reader đã từ chối yêu cầu hoàn thành của User.";
    }
}
