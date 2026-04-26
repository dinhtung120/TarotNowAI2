using MediatR;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.RespondConversationAddMoney;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class RespondConversationAddMoneyCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenAcceptMessageFails_ShouldCompensateFrozenOffer()
    {
        var requesterId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();

        var conversationRepo = new Mock<IConversationRepository>();
        var chatMessageRepo = new Mock<IChatMessageRepository>();
        var financeRepo = new Mock<IChatFinanceRepository>();
        var walletRepo = new Mock<IWalletRepository>();
        var transactionCoordinator = new Mock<ITransactionCoordinator>();
        var mediator = new Mock<IMediator>();
        var domainEventPublisher = new Mock<IDomainEventPublisher>();

        transactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

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
        mediator
            .Setup(x => x.Send(It.IsAny<SendMessageCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("mongo write failed"));

        var frozenItem = new ChatQuestionItem
        {
            Id = itemId,
            FinanceSessionId = sessionId,
            AmountDiamond = 25,
            PayerId = requesterId,
            ReceiverId = readerId,
            Status = QuestionItemStatus.Accepted
        };
        financeRepo
            .Setup(x => x.GetItemForUpdateAsync(itemId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(frozenItem);
        financeRepo
            .Setup(x => x.GetSessionForUpdateAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatFinanceSession
            {
                Id = sessionId,
                ConversationRef = conversation.Id,
                UserId = requesterId,
                ReaderId = readerId,
                Status = ChatFinanceSessionStatus.Active,
                TotalFrozen = 25
            });

        var handler = new RespondConversationAddMoneyCommandHandler(
            conversationRepo.Object,
            chatMessageRepo.Object,
            financeRepo.Object,
            walletRepo.Object,
            transactionCoordinator.Object,
            mediator.Object,
            domainEventPublisher.Object);

        var command = new RespondConversationAddMoneyCommand
        {
            ConversationId = conversation.Id,
            UserId = requesterId,
            Accept = true,
            OfferMessageId = offer.Id
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        walletRepo.Verify(
            x => x.RefundAsync(
                requesterId,
                25,
                "offer_accept_compensation",
                itemId.ToString(),
                It.IsAny<string>(),
                null,
                $"compensate_offer_accept_{itemId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
        financeRepo.Verify(x => x.UpdateItemAsync(It.Is<ChatQuestionItem>(item =>
            item.Id == itemId
            && item.Status == QuestionItemStatus.Refunded
            && item.RefundedAt != null), It.IsAny<CancellationToken>()), Times.Once);
        financeRepo.Verify(x => x.UpdateSessionAsync(It.Is<ChatFinanceSession>(session =>
            session.Id == sessionId
            && session.TotalFrozen == 0
            && session.Status == ChatFinanceSessionStatus.Refunded), It.IsAny<CancellationToken>()), Times.Once);
        financeRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.Is<MoneyChangedDomainEvent>(eventData =>
                eventData.UserId == requesterId
                && eventData.Currency == CurrencyType.Diamond
                && eventData.ChangeType == TransactionType.EscrowRefund
                && eventData.DeltaAmount == 25
                && eventData.ReferenceId == itemId.ToString()),
            It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.IsAny<ConversationUpdatedDomainEvent>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}
