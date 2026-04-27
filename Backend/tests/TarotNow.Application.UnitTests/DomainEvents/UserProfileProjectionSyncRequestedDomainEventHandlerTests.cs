using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.DomainEvents;

public sealed class UserProfileProjectionSyncRequestedDomainEventHandlerTests
{
    private readonly Mock<IReaderProfileRepository> _readerProfileRepository = new();
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyService = new();
    private readonly UserProfileProjectionSyncRequestedDomainEventHandler _handler;

    public UserProfileProjectionSyncRequestedDomainEventHandlerTests()
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

        _handler = new UserProfileProjectionSyncRequestedDomainEventHandler(
            _readerProfileRepository.Object,
            _idempotencyService.Object);
    }

    [Fact]
    public async Task Handle_ShouldSkip_WhenIncomingVersionIsStale()
    {
        var userId = Guid.NewGuid();
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReaderProfileDto
            {
                UserId = userId.ToString(),
                DisplayName = "Newest Name",
                AvatarUrl = "https://cdn/new-avatar.png",
                UpdatedAt = DateTime.UtcNow
            });

        var domainEvent = new UserProfileProjectionSyncRequestedDomainEvent
        {
            UserId = userId,
            DisplayName = "Old Name",
            AvatarUrl = "https://cdn/old-avatar.png",
            SourceUpdatedAtUtc = DateTime.UtcNow.AddMinutes(-5),
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserProfileProjectionSyncRequestedDomainEvent>(domainEvent, null, domainEvent.EventIdempotencyKey),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<ReaderProfileDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdate_WhenIncomingVersionIsNewer()
    {
        var userId = Guid.NewGuid();
        var profile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            DisplayName = "Before",
            AvatarUrl = "https://cdn/before.png",
            UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
        };
        _readerProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(userId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var domainEvent = new UserProfileProjectionSyncRequestedDomainEvent
        {
            UserId = userId,
            DisplayName = "After",
            AvatarUrl = "https://cdn/after.png",
            SourceUpdatedAtUtc = DateTime.UtcNow,
            OccurredAtUtc = DateTime.UtcNow
        };

        await _handler.Handle(
            new DomainEventNotification<UserProfileProjectionSyncRequestedDomainEvent>(domainEvent, null, domainEvent.EventIdempotencyKey),
            CancellationToken.None);

        _readerProfileRepository.Verify(
            repository => repository.UpdateAsync(
                It.Is<ReaderProfileDto>(updated =>
                    updated.UserId == userId.ToString()
                    && updated.DisplayName == "After"
                    && updated.AvatarUrl == "https://cdn/after.png"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
