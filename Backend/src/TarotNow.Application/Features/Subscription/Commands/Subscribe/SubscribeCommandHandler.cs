using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

// DTO cấu hình một entitlement trong JSON quyền lợi của gói.
public class EntitlementConfigDto
{
    // Khóa entitlement.
    public string key { get; set; } = string.Empty;

    // Hạn mức mỗi ngày của entitlement.
    public int dailyQuota { get; set; }
}

// Handler điều phối luồng mua subscription.
public partial class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler subscribe.
    /// Luồng xử lý: nhận repository/service để xử lý transaction mua gói, trừ ví và phát domain event kích hoạt subscription.
    /// </summary>
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

    /// <summary>
    /// Xử lý command mua subscription.
    /// Luồng xử lý: bọc toàn bộ quy trình mua trong transaction coordinator để đảm bảo trừ ví, tạo subscription và cấp bucket cùng thành công.
    /// </summary>
    public async Task<Guid> Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var createdSubscriptionId = Guid.Empty;
        await _transactionCoordinator.ExecuteAsync(
            async ct =>
            {
                createdSubscriptionId = await ExecuteSubscribeAsync(request, ct);
                // Lưu subscription id tạo mới trong scope transaction để trả về sau commit.
            },
            cancellationToken);

        return createdSubscriptionId;
    }
}
