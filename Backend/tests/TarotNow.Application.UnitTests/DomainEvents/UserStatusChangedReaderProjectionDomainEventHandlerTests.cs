using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.DomainEvents;

public sealed class UserStatusChangedReaderProjectionDomainEventHandlerTests
{
    private readonly Mock<IReaderProfileRepository> _readerProfileRepository = new();
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyService = new();
    private readonly UserStatusChangedReaderProjectionDomainEventHandler _handler;

    public UserStatusChangedReaderProjectionDomainEventHandlerTests()
    {
        _idempotencyService
            .Setup(service => service.HasProcessedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _idempotencyService
            .Setup(service => service.HasProcessedInlineEventAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _idempotencyService
            .Setup(service => service.MarkProcessedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _idempotencyService
            .Setup(service => service.MarkInlineEventProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _handler = new UserStatusChangedReaderProjectionDomainEventHandler(
            _readerProfileRepository.Object,
            _idempotencyService.Object);
    }

    [Fact]
    public async Task Handle_ShouldPromoteOfflineReader_ToOnline()
    {
        var userId = Guid.NewGuid().ToString();
        var profile = new ReaderProfileDto
        {
            UserId = userId,
            Status = "offline"
        };
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var domainEvent = new UserStatusChangedDomainEvent
        {
            UserId = userId,
            Status = "online",
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserStatusChangedDomainEvent>(domainEvent),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(
                It.Is<ReaderProfileDto>(updated => updated.UserId == userId && updated.Status == "online"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotOverrideBusy_WhenOnlineEventArrives()
    {
        var userId = Guid.NewGuid().ToString();
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReaderProfileDto
            {
                UserId = userId,
                Status = "busy"
            });

        var domainEvent = new UserStatusChangedDomainEvent
        {
            UserId = userId,
            Status = "online",
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserStatusChangedDomainEvent>(domainEvent),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<ReaderProfileDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldDowngradeBusy_ToOfflineWhenDisconnected()
    {
        var userId = Guid.NewGuid().ToString();
        var profile = new ReaderProfileDto
        {
            UserId = userId,
            Status = "busy"
        };
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var domainEvent = new UserStatusChangedDomainEvent
        {
            UserId = userId,
            Status = "offline",
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserStatusChangedDomainEvent>(domainEvent),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(
                It.Is<ReaderProfileDto>(updated => updated.UserId == userId && updated.Status == "offline"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSkip_WhenReaderProfileDoesNotExist()
    {
        var userId = Guid.NewGuid().ToString();
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ReaderProfileDto?)null);

        var domainEvent = new UserStatusChangedDomainEvent
        {
            UserId = userId,
            Status = "online",
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserStatusChangedDomainEvent>(domainEvent),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<ReaderProfileDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
