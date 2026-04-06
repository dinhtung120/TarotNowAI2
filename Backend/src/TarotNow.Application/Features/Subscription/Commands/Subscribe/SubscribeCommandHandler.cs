using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public class EntitlementConfigDto
{
    public string key { get; set; } = string.Empty;
    public int dailyQuota { get; set; }
}

public partial class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public SubscribeCommandHandler(
        ISubscriptionRepository subscriptionRepository,
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        ITransactionCoordinator transactionCoordinator,
        IDomainEventPublisher domainEventPublisher)
    {
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _transactionCoordinator = transactionCoordinator;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<Guid> Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var createdSubscriptionId = Guid.Empty;
        await _transactionCoordinator.ExecuteAsync(async ct =>
        {
            createdSubscriptionId = await ExecuteSubscribeAsync(request, ct);
        }, cancellationToken);
        return createdSubscriptionId;
    }
}
