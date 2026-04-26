using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class ConversationAddMoneyAcceptedSyncRequestedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_WhenMessageNotProjected_ShouldPersistMessageUpdateConversationAndPublishRealtimeEvents()
    {
        var eventId = Guid.NewGuid();
        var conversation = new ConversationDto
        {
            Id = "conv-1",
            UserId = Guid.NewGuid().ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
            UnreadCountReader = 0,
            LastMessageAt = DateTime.UtcNow.AddMinutes(-10)
        };
        var domainEvent = new ConversationAddMoneyAcceptedSyncRequestedDomainEvent(
            conversation.Id,
            conversation.UserId,
            offerMessageId: "offer-1",
            proposalId: "proposal-1",
            responseMessageId: "507f1f77bcf86cd799439011",
            occurredAtUtc: DateTime.UtcNow);

        var conversationRepository = new Mock<IConversationRepository>();
        conversationRepository
            .Setup(x => x.GetByIdAsync(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);

        var chatMessageRepository = new Mock<IChatMessageRepository>();
        chatMessageRepository
            .Setup(x => x.GetByIdAsync(domainEvent.ResponseMessageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatMessageDto?)null);

        ChatMessageDto? savedMessage = null;
        chatMessageRepository
            .Setup(x => x.AddAsync(It.IsAny<ChatMessageDto>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageDto, CancellationToken>((message, _) => savedMessage = message)
            .Returns(Task.CompletedTask);

        var domainEventPublisher = new Mock<IDomainEventPublisher>();
        var idempotencyService = new Mock<IEventHandlerIdempotencyService>();
        idempotencyService
            .Setup(x => x.HasProcessedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        idempotencyService
            .Setup(x => x.HasProcessedInlineEventAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        idempotencyService
            .Setup(x => x.MarkProcessedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        idempotencyService
            .Setup(x => x.MarkInlineEventProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var logger = new Mock<Microsoft.Extensions.Logging.ILogger<ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler>>();
        var handler = new ConversationAddMoneyAcceptedSyncRequestedDomainEventHandler(
            conversationRepository.Object,
            chatMessageRepository.Object,
            domainEventPublisher.Object,
            logger.Object,
            idempotencyService.Object);

        var notification = new DomainEventNotification<ConversationAddMoneyAcceptedSyncRequestedDomainEvent>(
            domainEvent,
            eventId,
            domainEvent.EventIdempotencyKey);

        await handler.Handle(notification, CancellationToken.None);

        Assert.NotNull(savedMessage);
        Assert.Equal(domainEvent.ResponseMessageId, savedMessage!.Id);
        Assert.Equal(ChatMessageType.PaymentAccept, savedMessage.Type);
        Assert.Equal(domainEvent.EventIdempotencyKey, savedMessage.SystemEventKey);
        Assert.Equal(1, conversation.UnreadCountReader);

        conversationRepository.Verify(x => x.UpdateAsync(conversation, It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.Is<ChatMessageCreatedDomainEvent>(eventData =>
                eventData.ConversationId == conversation.Id
                && eventData.MessageId == domainEvent.ResponseMessageId
                && eventData.MessageType == ChatMessageType.PaymentAccept),
            It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.IsAny<UnreadCountChangedDomainEvent>(),
            It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.Is<ConversationUpdatedDomainEvent>(eventData =>
                eventData.ConversationId == conversation.Id
                && eventData.Type == "add_money_responded"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
