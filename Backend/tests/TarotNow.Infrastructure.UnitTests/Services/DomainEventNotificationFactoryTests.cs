using MediatR;
using Moq;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.Services;

namespace TarotNow.Infrastructure.UnitTests.Services;

public sealed class DomainEventNotificationFactoryTests
{
    [Fact]
    public async Task InlineMediatRDomainEventDispatcher_ShouldCreateNotification_ForRepresentativeDomainEvent()
    {
        var mediator = new Mock<IMediator>();
        DomainEventNotification<MoneyChangedDomainEvent>? captured = null;
        mediator
            .Setup(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, _) =>
            {
                captured = Assert.IsType<DomainEventNotification<MoneyChangedDomainEvent>>(notification);
            })
            .Returns(Task.CompletedTask);
        var dispatcher = new InlineMediatRDomainEventDispatcher(mediator.Object);
        var domainEvent = new MoneyChangedDomainEvent
        {
            UserId = Guid.NewGuid(),
            Currency = "gold",
            ChangeType = "credit",
            ReferenceId = "ref-1"
        };

        await dispatcher.PublishAsync(domainEvent);

        Assert.NotNull(captured);
        Assert.Same(domainEvent, captured!.DomainEvent);
    }

    [Fact]
    public void DomainEventNotifications_ShouldExposeExpectedConstructorShape()
    {
        var failures = typeof(IDomainEvent)
            .Assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(IDomainEvent).IsAssignableFrom(type))
            .Select(type => new
            {
                EventType = type,
                Constructor = typeof(DomainEventNotification<>).MakeGenericType(type)
                    .GetConstructor(new[] { type, typeof(Guid?), typeof(string) })
            })
            .Where(item => item.Constructor is null)
            .Select(item => item.EventType.FullName)
            .ToArray();

        Assert.Empty(failures);
    }
}
