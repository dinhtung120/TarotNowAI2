using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public partial class SubscribeCommandHandler
{
    private async Task<Guid> ExecuteSubscribeAsync(SubscribeCommand request, CancellationToken ct)
    {
        var existingSub = await _subscriptionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey, ct);
        if (existingSub != null) return existingSub.Id;

        var plan = await GetActivePlanAsync(request.PlanId, ct);
        var user = await GetExistingUserAsync(request.UserId, ct);
        await EnsureNoSamePlanActiveAsync(request.UserId, request.PlanId, ct);
        await ChargeSubscriptionAsync(user.Id, plan, request.IdempotencyKey, ct);

        var now = DateTime.UtcNow;
        var subscription = CreateSubscription(user.Id, plan, request.IdempotencyKey, now);
        await _subscriptionRepository.AddSubscriptionAsync(subscription, ct);
        await AddEntitlementBucketsAsync(user.Id, subscription, plan, now, ct);

        await _userRepository.UpdateAsync(user, ct);
        await _subscriptionRepository.SaveChangesAsync(ct);
        await _domainEventPublisher.PublishAsync(new SubscriptionActivatedDomainEvent(user.Id, subscription.Id, plan.Id), ct);
        return subscription.Id;
    }

    private async Task<SubscriptionPlan> GetActivePlanAsync(Guid planId, CancellationToken ct)
    {
        var plan = await _subscriptionRepository.GetPlanByIdAsync(planId, ct);
        if (plan == null || !plan.IsActive) throw new NotFoundException("Gói đăng ký không tồn tại hoặc đã ngừng bán.");
        return plan;
    }

    private async Task<User> GetExistingUserAsync(Guid userId, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct);
        return user ?? throw new NotFoundException("Người dùng không tồn tại.");
    }

    private async Task EnsureNoSamePlanActiveAsync(Guid userId, Guid planId, CancellationToken ct)
    {
        var activeSubscriptions = await _subscriptionRepository.GetActiveSubscriptionsAsync(userId, ct);
        if (activeSubscriptions.Any(s => s.PlanId == planId))
        {
            throw new BadRequestException("Bạn đã có gói này đang hoạt động. Vui lòng chờ hết hạn trước khi mua lại.");
        }
    }

    private async Task ChargeSubscriptionAsync(Guid userId, SubscriptionPlan plan, string requestIdempotencyKey, CancellationToken ct)
    {
        try
        {
            await _walletRepository.DebitAsync(
                userId: userId,
                currency: CurrencyType.Diamond,
                type: TransactionType.Subscription,
                amount: plan.PriceDiamond,
                referenceSource: "buy_subscription",
                referenceId: plan.Id.ToString(),
                description: $"Mua gói đặc quyền: {plan.Name}",
                metadataJson: null,
                idempotencyKey: $"sub_buy_{requestIdempotencyKey}",
                cancellationToken: ct);
        }
        catch (Exception ex)
        {
            throw new BadRequestException($"Giao dịch Mua Gói Thất Bại Không Đủ Tiền Mua Hoặc Lỗi Cổng SQL Trừ: {ex.Message}");
        }
    }

}
