using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public partial class SubscribeCommandHandler
{
    /// <summary>
    /// Thực thi luồng mua subscription trong transaction.
    /// Luồng xử lý: kiểm tra idempotency, validate plan/user, trừ ví, tạo subscription + bucket entitlement, lưu thay đổi và phát domain event.
    /// </summary>
    private async Task<Guid> ExecuteSubscribeAsync(SubscribeCommand request, CancellationToken ct)
    {
        var existingSub = await _subscriptionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey, ct);
        if (existingSub is not null)
        {
            // Idempotent retry: đã xử lý trước đó thì trả lại subscription id cũ, không trừ ví hay tạo bản ghi mới.
            return existingSub.Id;
        }

        var plan = await GetActivePlanAsync(request.PlanId, ct);
        var user = await GetExistingUserAsync(request.UserId, ct);
        await EnsureNoSamePlanActiveAsync(request.UserId, request.PlanId, ct);
        await ChargeSubscriptionAsync(user.Id, plan, request.IdempotencyKey, ct);
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = user.Id,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.Subscription,
                DeltaAmount = -plan.PriceDiamond,
                ReferenceId = plan.Id.ToString()
            },
            ct);
        // Chuỗi check + charge phải hoàn tất trước khi tạo subscription để tránh cấp quyền khi chưa thanh toán thành công.

        var now = DateTime.UtcNow;
        var subscription = CreateSubscription(user.Id, plan, request.IdempotencyKey, now);
        await _subscriptionRepository.AddSubscriptionAsync(subscription, ct);
        await AddEntitlementBucketsAsync(user.Id, subscription, plan, now, ct);
        // Tạo subscription và bucket quyền lợi trong cùng transaction để dữ liệu entitlement luôn đồng bộ.

        await _userRepository.UpdateAsync(user, ct);
        await _subscriptionRepository.SaveChangesAsync(ct);
        // Commit dữ liệu persistence trước khi phát event để consumer không đọc trạng thái chưa lưu.

        await _domainEventPublisher.PublishAsync(
            new SubscriptionActivatedDomainEvent(user.Id, subscription.Id, plan.Id),
            ct);
        // Phát event kích hoạt sau khi dữ liệu đã sẵn sàng cho downstream xử lý.

        return subscription.Id;
    }

    /// <summary>
    /// Lấy gói subscription đang active theo id.
    /// Luồng xử lý: tải plan theo id và chặn khi plan không tồn tại hoặc đã ngừng bán.
    /// </summary>
    private async Task<SubscriptionPlan> GetActivePlanAsync(Guid planId, CancellationToken ct)
    {
        var plan = await _subscriptionRepository.GetPlanByIdAsync(planId, ct);
        if (plan is null || !plan.IsActive)
        {
            // Business rule: chỉ cho mua gói còn active để tránh giao dịch vào sản phẩm đã đóng bán.
            throw new NotFoundException("Gói đăng ký không tồn tại hoặc đã ngừng bán.");
        }

        return plan;
    }

    /// <summary>
    /// Lấy user hiện hữu theo id.
    /// Luồng xử lý: tải user và ném NotFound khi không tồn tại.
    /// </summary>
    private async Task<User> GetExistingUserAsync(Guid userId, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct);
        return user ?? throw new NotFoundException("Người dùng không tồn tại.");
    }

    /// <summary>
    /// Đảm bảo user chưa có gói cùng loại đang active.
    /// Luồng xử lý: lấy danh sách active subscriptions và chặn nếu đã tồn tại plan trùng.
    /// </summary>
    private async Task EnsureNoSamePlanActiveAsync(Guid userId, Guid planId, CancellationToken ct)
    {
        var activeSubscriptions = await _subscriptionRepository.GetActiveSubscriptionsAsync(userId, ct);
        if (activeSubscriptions.Any(subscription => subscription.PlanId == planId))
        {
            // Business rule: không cho mua chồng cùng gói khi gói hiện tại chưa hết hạn.
            throw new BadRequestException("Bạn đã có gói này đang hoạt động. Vui lòng chờ hết hạn trước khi mua lại.");
        }
    }

    /// <summary>
    /// Trừ ví diamond để thanh toán gói subscription.
    /// Luồng xử lý: gọi debit theo idempotency key giao dịch; ánh xạ lỗi sang BadRequest nghiệp vụ.
    /// </summary>
    private async Task ChargeSubscriptionAsync(
        Guid userId,
        SubscriptionPlan plan,
        string requestIdempotencyKey,
        CancellationToken ct)
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
            // Chuẩn hóa lỗi trừ ví sang thông điệp nghiệp vụ để client hiển thị nhất quán.
            throw new BadRequestException($"Giao dịch Mua Gói Thất Bại Không Đủ Tiền Mua Hoặc Lỗi Cổng SQL Trừ: {ex.Message}");
        }
    }
}
