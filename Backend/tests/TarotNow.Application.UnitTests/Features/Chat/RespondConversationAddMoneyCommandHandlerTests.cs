using System.Security.Cryptography;
using System.Text;
using MediatR;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_WhenAccept_ShouldPublishDurableSyncEventAndReturnReservedMessageId()
    {
        var requesterId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        var conversationRepo = new Mock<IConversationRepository>();
        var chatMessageRepo = new Mock<IChatMessageRepository>();
        var mediator = new Mock<IMediator>();
        var domainEventPublisher = new Mock<IDomainEventPublisher>();

        var conversation = new ConversationDto
        {
            Id = "conv-1",
            UserId = requesterId.ToString(),
            ReaderId = readerId.ToString(),
            Status = ConversationStatus.Ongoing
        };
        conversationRepo
            .Setup(x => x.GetByIdAsync(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);

        var offer = new ChatMessageDto
        {
            Id = "offer-1",
            ConversationId = conversation.Id,
            SenderId = readerId.ToString(),
            Type = ChatMessageType.PaymentOffer,
            PaymentPayload = new PaymentPayloadDto
            {
                AmountDiamond = 25,
                ProposalId = "proposal-1",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            }
        };
        chatMessageRepo
            .Setup(x => x.GetByIdAsync(offer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(offer);
        chatMessageRepo
            .Setup(x => x.HasPaymentOfferResponseAsync(conversation.Id, offer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        mediator
            .Setup(x => x.Send(It.IsAny<AddQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemId);

        var handler = new RespondConversationAddMoneyCommandHandlerRequestedDomainEventHandler(
            conversationRepo.Object,
            chatMessageRepo.Object,
            mediator.Object,
            domainEventPublisher.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());

        var command = new RespondConversationAddMoneyCommand
        {
            ConversationId = conversation.Id,
            UserId = requesterId,
            Accept = true,
            OfferMessageId = offer.Id
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Accepted);
        Assert.Equal(itemId, result.ItemId);
        var expectedMessageId = ComputeDeterministicMessageId(conversation.Id, offer.Id);
        Assert.Equal(expectedMessageId, result.MessageId);

        mediator.Verify(x => x.Send(It.IsAny<AddQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.VerifyNoOtherCalls();

        domainEventPublisher.Verify(x => x.PublishAsync(
            It.Is<ConversationAddMoneyAcceptedSyncRequestedDomainEvent>(eventData =>
                eventData.ConversationId == conversation.Id
                && eventData.SenderUserId == requesterId.ToString()
                && eventData.OfferMessageId == offer.Id
                && eventData.ProposalId == offer.PaymentPayload!.ProposalId
                && eventData.ResponseMessageId == result.MessageId),
            It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.VerifyNoOtherCalls();
    }

    private static string ComputeDeterministicMessageId(string conversationId, string offerMessageId)
    {
        var key = $"{conversationId}:{offerMessageId}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        Span<byte> bytes = stackalloc byte[12];
        hash.AsSpan(0, 12).CopyTo(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
