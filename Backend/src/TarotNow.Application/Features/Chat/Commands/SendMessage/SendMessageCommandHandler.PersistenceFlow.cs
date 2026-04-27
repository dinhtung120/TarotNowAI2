using System;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    private async Task<ChatMessageDto> PersistMessageFlowAsync(
        PersistMessageFlowContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await _messageRepo.AddAsync(context.Message, cancellationToken);
            await TryMarkReaderRepliedAsync(
                context.Conversation,
                context.SenderId,
                context.Request.Type,
                cancellationToken);

            IncrementUnreadCounter(context.Conversation, context.SenderId);
            await UpdateConversationTimelineAsync(context, cancellationToken);
            await _conversationRepo.UpdateAsync(context.Conversation, cancellationToken);

            await PublishFreezeEventsAsync(
                context.Request.SenderId,
                context.Conversation,
                context.FirstMessageFreeze,
                cancellationToken);
            await PublishRealtimeEventsAsync(context.Conversation, context.Message, cancellationToken);

            return context.Message;
        }
        catch
        {
            if (context.FirstMessageFreeze.IsApplied)
            {
                await CompensateMainQuestionFreezeAsync(
                    context.Conversation,
                    context.Request.SenderId,
                    context.FirstMessageFreeze.FreezeItemIdempotencyKey!,
                    cancellationToken);
            }

            throw;
        }
    }

    private async Task UpdateConversationTimelineAsync(
        PersistMessageFlowContext context,
        CancellationToken cancellationToken)
    {
        if (!context.FirstMessageFreeze.IsApplied)
        {
            context.Conversation.LastMessageAt = context.Message.CreatedAt;
            context.Conversation.UpdatedAt = context.Message.CreatedAt;
            return;
        }

        var systemMessage = BuildSystemMessage(
            context.Conversation.Id,
            context.SenderId,
            $"Đã đóng băng {context.FirstMessageFreeze.AmountDiamond} 💎 cho cuộc chat này. Đang chờ Reader phản hồi.");
        await _messageRepo.AddAsync(systemMessage, cancellationToken);
        context.Conversation.LastMessageAt = systemMessage.CreatedAt;
        context.Conversation.UpdatedAt = systemMessage.CreatedAt;
    }

    private async Task PublishFreezeEventsAsync(
        Guid senderId,
        ConversationDto conversation,
        FirstMessageFreezeResult firstMessageFreeze,
        CancellationToken cancellationToken)
    {
        if (!firstMessageFreeze.IsApplied)
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(new Domain.Events.MoneyChangedDomainEvent
        {
            UserId = senderId,
            Currency = Domain.Enums.CurrencyType.Diamond,
            ChangeType = Domain.Enums.TransactionType.EscrowFreeze,
            DeltaAmount = -firstMessageFreeze.AmountDiamond,
            ReferenceId = conversation.Id
        }, cancellationToken);

        if (!Guid.TryParse(conversation.ReaderId, out var readerGuid))
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(new Domain.Events.ChatOfferReceivedDomainEvent
        {
            ConversationId = conversation.Id,
            ReaderId = readerGuid,
            UserId = senderId,
            OfferExpiresAtUtc = firstMessageFreeze.OfferExpiresAtUtc!.Value
        }, cancellationToken);
    }

    private async Task PublishRealtimeEventsAsync(
        ConversationDto conversation,
        ChatMessageDto message,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ChatMessageCreatedDomainEvent
            {
                ConversationId = conversation.Id,
                MessageId = message.Id,
                SenderId = message.SenderId,
                MessageType = message.Type,
                OccurredAtUtc = message.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "message_created", message.CreatedAt),
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.UnreadCountChangedDomainEvent
            {
                ConversationId = conversation.Id,
                UserId = conversation.UserId,
                ReaderId = conversation.ReaderId,
                OccurredAtUtc = message.CreatedAt
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ChatModerationRequestedDomainEvent
            {
                MessageId = message.Id,
                ConversationId = conversation.Id,
                SenderId = message.SenderId,
                MessageType = message.Type,
                Content = message.Content,
                CreatedAtUtc = message.CreatedAt
            },
            cancellationToken);
    }
}
