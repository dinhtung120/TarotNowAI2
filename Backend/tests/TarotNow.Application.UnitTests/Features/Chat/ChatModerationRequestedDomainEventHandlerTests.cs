using Moq;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Chat;

public sealed class ChatModerationRequestedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenModerationDisabled()
    {
        var (handler, _, _, _) = BuildHandler(enabled: false, keywords: ["scam"]);
        var domainEvent = BuildDomainEvent(content: "this is scam", messageType: "text");
        var notification = new DomainEventNotification<ChatModerationRequestedDomainEvent>(
            domainEvent,
            Guid.NewGuid(),
            EventIdempotencyKey: null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(notification, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldNoOp_WhenMessageTypeIsNotModerated()
    {
        var (handler, reportRepository, messageRepository, _) = BuildHandler(enabled: true, keywords: ["scam"]);
        var domainEvent = BuildDomainEvent(content: "this is scam", messageType: "image");
        var notification = new DomainEventNotification<ChatModerationRequestedDomainEvent>(
            domainEvent,
            Guid.NewGuid(),
            EventIdempotencyKey: null);

        await handler.Handle(notification, CancellationToken.None);

        messageRepository.Verify(x => x.UpdateFlagAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        reportRepository.Verify(x => x.AddAsync(It.IsAny<ReportDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFlagAndCreateReport_WhenKeywordMatched()
    {
        var (handler, reportRepository, messageRepository, _) = BuildHandler(enabled: true, keywords: ["scam", "fraud"]);
        var domainEvent = BuildDomainEvent(content: "Potential SCAM content", messageType: "text");
        var notification = new DomainEventNotification<ChatModerationRequestedDomainEvent>(
            domainEvent,
            Guid.NewGuid(),
            EventIdempotencyKey: null);

        await handler.Handle(notification, CancellationToken.None);

        messageRepository.Verify(
            x => x.UpdateFlagAsync(domainEvent.MessageId, true, It.IsAny<CancellationToken>()),
            Times.Once);
        reportRepository.Verify(
            x => x.AddAsync(
                It.Is<ReportDto>(report =>
                    report.TargetType == "message"
                    && report.TargetId == domainEvent.MessageId
                    && report.ConversationRef == domainEvent.ConversationId
                    && report.Reason.Contains("scam", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNoOp_WhenNoKeywordMatched()
    {
        var (handler, reportRepository, messageRepository, _) = BuildHandler(enabled: true, keywords: ["scam"]);
        var domainEvent = BuildDomainEvent(content: "hello world", messageType: "text");
        var notification = new DomainEventNotification<ChatModerationRequestedDomainEvent>(
            domainEvent,
            Guid.NewGuid(),
            EventIdempotencyKey: null);

        await handler.Handle(notification, CancellationToken.None);

        messageRepository.Verify(x => x.UpdateFlagAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        reportRepository.Verify(x => x.AddAsync(It.IsAny<ReportDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private static (
        ChatModerationRequestedDomainEventHandler Handler,
        Mock<IReportRepository> ReportRepository,
        Mock<IChatMessageRepository> MessageRepository,
        Mock<IEventHandlerIdempotencyService> IdempotencyService) BuildHandler(
        bool enabled,
        IReadOnlyCollection<string> keywords)
    {
        var reportRepository = new Mock<IReportRepository>();
        reportRepository
            .Setup(x => x.AddAsync(It.IsAny<ReportDto>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var messageRepository = new Mock<IChatMessageRepository>();
        messageRepository
            .Setup(x => x.UpdateFlagAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var settings = new Mock<IChatModerationSettings>();
        settings.SetupGet(x => x.Enabled).Returns(enabled);
        settings.SetupGet(x => x.Keywords).Returns(keywords);

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

        var logger = Mock.Of<ILogger<ChatModerationRequestedDomainEventHandler>>();
        var handler = new ChatModerationRequestedDomainEventHandler(
            reportRepository.Object,
            messageRepository.Object,
            settings.Object,
            logger,
            idempotencyService.Object);

        return (handler, reportRepository, messageRepository, idempotencyService);
    }

    private static ChatModerationRequestedDomainEvent BuildDomainEvent(string content, string messageType)
    {
        return new ChatModerationRequestedDomainEvent
        {
            MessageId = "msg-1",
            ConversationId = "conv-1",
            SenderId = "sender-1",
            MessageType = messageType,
            Content = content,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
