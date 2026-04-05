using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private async Task<FirstMessageFreezeResult> TryFreezeMainQuestionOnFirstUserMessageAsync(
        ConversationDto conversation,
        string senderId,
        string messageType,
        CancellationToken cancellationToken)
    {
        if (conversation.Status != ConversationStatus.Pending || senderId != conversation.UserId)
        {
            return FirstMessageFreezeResult.None;
        }

        // FIX: Bỏ qua giam tiền nếu người dùng gọi Call và hệ thống cắm trả tin nhắn Log
        if (messageType == ChatMessageType.CallLog)
        {
            return FirstMessageFreezeResult.None;
        }

        var ids = ParseConversationParticipants(conversation);
        var readerProfile = await _readerProfileRepo.GetByUserIdAsync(conversation.ReaderId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy hồ sơ Reader.");

        if (readerProfile.DiamondPerQuestion <= 0)
        {
            throw new BadRequestException("Giá câu hỏi của Reader không hợp lệ.");
        }

        var amountDiamond = readerProfile.DiamondPerQuestion;
        var offerExpiresAtUtc = DateTime.UtcNow.AddHours(ResolveAwaitingAcceptanceHours(conversation.SlaHours));
        var idempotencyKey = $"main_question_{conversation.Id}";
        var freezeContext = new MainQuestionFreezeContext(
            ids.UserId,
            ids.ReaderId,
            amountDiamond,
            offerExpiresAtUtc,
            idempotencyKey);

        await _transactionCoordinator.ExecuteAsync(
            transactionCt => FreezeMainQuestionAsync(conversation, freezeContext, transactionCt),
            cancellationToken);

        return new FirstMessageFreezeResult(true, freezeContext.AmountDiamond, freezeContext.OfferExpiresAtUtc);
    }

    private static (Guid UserId, Guid ReaderId) ParseConversationParticipants(ConversationDto conversation)
    {
        if (!Guid.TryParse(conversation.UserId, out var userId))
        {
            throw new BadRequestException("UserId của cuộc trò chuyện không hợp lệ.");
        }

        if (!Guid.TryParse(conversation.ReaderId, out var readerId))
        {
            throw new BadRequestException("ReaderId của cuộc trò chuyện không hợp lệ.");
        }

        return (userId, readerId);
    }

    private static int ResolveAwaitingAcceptanceHours(int slaHours)
    {
        return slaHours <= 6 ? 6 : 12;
    }

    private static ChatMessageDto BuildSystemMessage(string conversationId, string senderId, string content)
    {
        return new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = senderId,
            Type = ChatMessageType.System,
            Content = content,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    private readonly record struct FirstMessageFreezeResult(
        bool IsTriggered,
        long AmountDiamond,
        DateTime? OfferExpiresAtUtc)
    {
        public static FirstMessageFreezeResult None => new(false, 0, null);
    }

    private readonly record struct MainQuestionFreezeContext(
        Guid UserId,
        Guid ReaderId,
        long AmountDiamond,
        DateTime OfferExpiresAtUtc,
        string IdempotencyKey);
}
