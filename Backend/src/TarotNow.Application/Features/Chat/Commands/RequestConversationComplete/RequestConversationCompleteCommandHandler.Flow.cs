using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
{
    private async Task<DateTime?> HandleFirstRequestIfNeededAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        if (context.IsFirstRequest == false)
        {
            return null;
        }

        var lastMessageAt = await CancelPendingAddMoneyOfferAsync(
            context.Conversation,
            context.RequesterId,
            cancellationToken);

        context.Conversation.Confirm!.RequestedBy = context.RequesterId;
        context.Conversation.Confirm.RequestedAt = context.Now;
        context.Conversation.Confirm.AutoResolveAt = context.Now.AddHours(context.IsUserRequester ? 12 : 48);

        return await AddSystemMessageAsync(
            context.Conversation,
            context.RequesterId,
            new SystemMessageSpec(
                ChatMessageType.System,
                BuildFirstRequestMessage(context.IsUserRequester),
                lastMessageAt),
            cancellationToken);
    }

    private static string BuildFirstRequestMessage(bool isUserRequester)
    {
        return isUserRequester
            ? "User đã yêu cầu hoàn thành cuộc trò chuyện. Nếu Reader không phản hồi, hệ thống sẽ tự động giải ngân sau tối đa 12 giờ."
            : "Reader đã đánh dấu hoàn thành. Nếu User không phản hồi, hệ thống sẽ tự động giải ngân sau tối đa 48 giờ.";
    }

    private static void ApplyRequesterConfirmation(RequestContext context)
    {
        if (context.IsUserRequester)
        {
            context.Conversation.Confirm!.UserAt = context.Now;
            return;
        }

        context.Conversation.Confirm!.ReaderAt = context.Now;
    }

    private static bool HasBothSidesConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }

    private async Task PersistPendingConversationAsync(
        ConversationDto conversation,
        DateTime now,
        DateTime? lastMessageAt,
        CancellationToken cancellationToken)
    {
        conversation.UpdatedAt = now;
        if (lastMessageAt.HasValue)
        {
            conversation.LastMessageAt = lastMessageAt.Value;
        }

        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
