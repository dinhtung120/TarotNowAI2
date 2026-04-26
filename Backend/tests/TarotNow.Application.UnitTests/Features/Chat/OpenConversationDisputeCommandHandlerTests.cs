using MediatR;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.OpenConversationDispute;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Chat;

public class OpenConversationDisputeCommandExecutorTests
{
    [Fact]
    public async Task Handle_WhenItemIdMissing_ShouldResolveLatestEligibleItemAndPublishDisputeEvent()
    {
        var requesterId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var acceptedItemId = Guid.NewGuid();
        var disputedItemId = Guid.NewGuid();

        var conversationRepo = new Mock<IConversationRepository>();
        var financeRepo = new Mock<IChatFinanceRepository>();
        var mediator = new Mock<IMediator>();
        var domainEventPublisher = new Mock<IDomainEventPublisher>();

        var conversation = new ConversationDto
        {
            Id = "conv-dispute-1",
            UserId = requesterId.ToString(),
            ReaderId = readerId.ToString(),
            Status = ConversationStatus.Ongoing
        };
        conversationRepo
            .Setup(x => x.GetByIdAsync(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);

        financeRepo
            .Setup(x => x.GetSessionByConversationRefAsync(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatFinanceSession
            {
                Id = sessionId,
                ConversationRef = conversation.Id,
                UserId = requesterId,
                ReaderId = readerId
            });
        financeRepo
            .Setup(x => x.GetItemsBySessionIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatQuestionItem>
            {
                new()
                {
                    Id = acceptedItemId,
                    FinanceSessionId = sessionId,
                    Status = QuestionItemStatus.Accepted,
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-2),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5)
                },
                new()
                {
                    Id = disputedItemId,
                    FinanceSessionId = sessionId,
                    Status = QuestionItemStatus.Disputed,
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-1),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-6)
                }
            });
        mediator
            .Setup(x => x.Send(It.IsAny<OpenDisputeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new OpenConversationDisputeCommandExecutor(
            conversationRepo.Object,
            financeRepo.Object,
            mediator.Object,
            domainEventPublisher.Object);

        var command = new OpenConversationDisputeCommand
        {
            ConversationId = conversation.Id,
            UserId = requesterId,
            Reason = "Lý do tranh chấp hợp lệ để test."
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(ConversationStatus.Disputed, result.Status);
        mediator.Verify(x => x.Send(
            It.Is<OpenDisputeCommand>(request =>
                request.ItemId == disputedItemId
                && request.UserId == requesterId
                && request.Reason == command.Reason),
            It.IsAny<CancellationToken>()), Times.Once);
        conversationRepo.Verify(x => x.UpdateAsync(
            It.Is<ConversationDto>(value =>
                value.Id == conversation.Id
                && value.Status == ConversationStatus.Disputed
                && value.UpdatedAt != null),
            It.IsAny<CancellationToken>()), Times.Once);
        domainEventPublisher.Verify(x => x.PublishAsync(
            It.Is<ConversationUpdatedDomainEvent>(eventData =>
                eventData.ConversationId == conversation.Id
                && eventData.Type == "disputed"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
