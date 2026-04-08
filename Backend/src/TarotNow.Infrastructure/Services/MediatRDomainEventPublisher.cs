using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

// Publisher chuyển DomainEvent sang MediatR notification để phát đi toàn hệ thống.
public sealed class MediatRDomainEventPublisher : IDomainEventPublisher
{
    // Bản đồ ánh xạ từng loại domain event sang notification tương ứng.
    private static readonly IReadOnlyDictionary<Type, Func<IDomainEvent, INotification>> NotificationFactories =
        new Dictionary<Type, Func<IDomainEvent, INotification>>
        {
            [typeof(EscrowReleasedDomainEvent)] = domainEvent =>
                new EscrowReleasedNotification((EscrowReleasedDomainEvent)domainEvent),
            [typeof(EscrowRefundedDomainEvent)] = domainEvent =>
                new EscrowRefundedNotification((EscrowRefundedDomainEvent)domainEvent),
            [typeof(ReadingBillingCompletedDomainEvent)] = domainEvent =>
                new ReadingBillingCompletedNotification((ReadingBillingCompletedDomainEvent)domainEvent),
            [typeof(ConversationUpdatedDomainEvent)] = domainEvent =>
                new ConversationUpdatedNotification((ConversationUpdatedDomainEvent)domainEvent),
            [typeof(SubscriptionActivatedDomainEvent)] = domainEvent =>
                new SubscriptionActivatedNotification((SubscriptionActivatedDomainEvent)domainEvent),
            [typeof(SubscriptionExpiredDomainEvent)] = domainEvent =>
                new SubscriptionExpiredNotification((SubscriptionExpiredDomainEvent)domainEvent),
            [typeof(EntitlementConsumedDomainEvent)] = domainEvent =>
                new EntitlementConsumedNotification((EntitlementConsumedDomainEvent)domainEvent),
            [typeof(ChatOfferReceivedDomainEvent)] = domainEvent =>
                new ChatOfferReceivedNotification((ChatOfferReceivedDomainEvent)domainEvent)
        };

    // Mediator dùng để dispatch notification đến các handler đã đăng ký.
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo publisher với mediator của ứng dụng.
    /// Luồng này tách trách nhiệm publish event khỏi logic nghiệp vụ domain.
    /// </summary>
    public MediatRDomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Publish domain event nếu có mapping notification phù hợp.
    /// Luồng bỏ qua event chưa map để tránh ném lỗi không cần thiết trong runtime.
    /// </summary>
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = MapNotification(domainEvent);
        return notification == null
            // Event chưa đăng ký mapping thì coi như no-op có chủ đích.
            ? Task.CompletedTask
            : _mediator.Publish(notification, cancellationToken);
    }

    /// <summary>
    /// Ánh xạ domain event sang notification cụ thể.
    /// Luồng lookup dictionary giúp mở rộng mapping dễ và hạn chế switch cồng kềnh.
    /// </summary>
    private static INotification? MapNotification(IDomainEvent domainEvent)
    {
        return NotificationFactories.TryGetValue(domainEvent.GetType(), out var factory)
            ? factory(domainEvent)
            : null;
    }
}
