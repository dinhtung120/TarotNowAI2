/*
 * ===================================================================
 * FILE: SubscribeCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Subscription.Commands.Subscribe
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý lõi việc Khách Hàng thanh toán Diamond để mua Subscription.
 *   
 *   CHI TIẾT:
 *   1. Chặn Idempotency (Chống trừ đúp thẻ)
 *   2. Đọc User, trừ đúng số dư Diamond (Domain logic Wallet).
 *   3. Lấy Danh Sách JSON Entitlements của Gói, chẻ nhỏ thành các Giỏ Nhựa (Bucket).
 *   4. Cuộn tất cả vào SQL Transaction duy nhất để đảm bảo Atomicity (Trừ Tiền Mà Rớt Giỏ Thẻ -> Rollback Tiền Ra Khách Lại).
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public class EntitlementConfigDto
{
    public string key { get; set; } = string.Empty;
    public int dailyQuota { get; set; }
}

public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Guid>
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
        Guid createdSubscriptionId = Guid.Empty;

        // Vòng Bọc An Toàn Transaction - Có Hủy Liền Chống Kẹt Băng Giao Dịch
        await _transactionCoordinator.ExecuteAsync(async (ct) =>
        {
            // 1. NGĂN CHẶN LỖI MOBILE BẤM 2 LẦN NÚT TRÙNG REQUEST.
            var existingSub = await _subscriptionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey, ct);
            if (existingSub != null)
            {
                createdSubscriptionId = existingSub.Id;
                return; // Có sẵn rồi thì nhả Id cũ ra luôn, khách đỡ khóc trừ x2 tài khoản.
            }

            // 2. TÌM VÀ NGÓ QUA GÓI CƯỚC ADMIN ĐANG BÁN.
            var plan = await _subscriptionRepository.GetPlanByIdAsync(request.PlanId, ct);
            if (plan == null || !plan.IsActive)
                throw new NotFoundException("Gói đăng ký không tồn tại hoặc đã ngừng bán.");

            // 3. TÌM KHÁCH HÀNG
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại.");

            // SCALE-1 FIX: Chặn mua trùng gói cùng loại nếu đang active.
            // Nếu user đã có gói cùng PlanId đang hoạt động, không cho mua thêm
            // → tránh tạo bucket x10 khi user bấm mua liên tục.
            var activeSubscriptions = await _subscriptionRepository.GetActiveSubscriptionsAsync(request.UserId, ct);
            var hasSamePlanActive = activeSubscriptions.Any(s => s.PlanId == request.PlanId);
            if (hasSamePlanActive)
                throw new BadRequestException("Bạn đã có gói này đang hoạt động. Vui lòng chờ hết hạn trước khi mua lại.");

            // 4. THAO TÁC RÚT TIỀN (VÍ KẾ TOÁN QUA WALLET REPOSITORY)
            var amountToCharge = plan.PriceDiamond;

            try
            {
                await _walletRepository.DebitAsync(
                    userId: user.Id,
                    currency: TarotNow.Domain.Enums.CurrencyType.Diamond,
                    type: TarotNow.Domain.Enums.TransactionType.Subscription,
                    amount: amountToCharge,
                    referenceSource: "buy_subscription",
                    referenceId: plan.Id.ToString(),
                    description: $"Mua gói đặc quyền: {plan.Name}",
                    metadataJson: null,
                    idempotencyKey: $"sub_buy_{request.IdempotencyKey}",
                    cancellationToken: ct);
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Giao dịch Mua Gói Thất Bại Không Đủ Tiền Mua Hoặc Lỗi Cổng SQL Trừ: {ex.Message}");
            }

            // 5. MỞ HỒ SƠ TỜ RƠI DỊCH VỤ MỚI.
            var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(plan.DurationDays);

            var userSubscription = new UserSubscription(
                userId: user.Id,
                planId: plan.Id,
                startDate: startDate,
                endDate: endDate,
                pricePaidDiamond: amountToCharge,
                idempotencyKey: request.IdempotencyKey
            );

            await _subscriptionRepository.AddSubscriptionAsync(userSubscription, ct);
            createdSubscriptionId = userSubscription.Id;

            // 6. XÉ GÓI GIỎ QUYỀN LỢI ĐỔ VÀO BỤNG TỪ JSON ADMIN LẮP.
            var entitlements = JsonSerializer.Deserialize<List<EntitlementConfigDto>>(plan.EntitlementsJson);
            if (entitlements != null && entitlements.Count > 0)
            {
                var buckets = new List<SubscriptionEntitlementBucket>();
                foreach (var config in entitlements)
                {
                    buckets.Add(new SubscriptionEntitlementBucket(
                        userSubscriptionId: userSubscription.Id,
                        userId: user.Id,
                        entitlementKey: config.key,
                        dailyQuota: config.dailyQuota,
                        currentDate: todayUtc,
                        subscriptionEndDate: endDate
                    ));
                }
                
                await _subscriptionRepository.AddBucketsAsync(buckets, ct);
            }

            // 7. LƯU CHỐT SỔ. CÚ NHẤN KHÔNG THỂ VÃN HỒI (SAVE NHẤT CỦA CỨNG).
            await _userRepository.UpdateAsync(user, ct); // Update Wallet Mới Trừ.
            await _subscriptionRepository.SaveChangesAsync(ct);
            
            // 8. ĐÁNH DẤU CHUÔNG THÔNG BÁO CHO TRỜI BIẾT "ĐÃ KÍCH HOẠT THÀNH CÔNG".
            await _domainEventPublisher.PublishAsync(new SubscriptionActivatedDomainEvent(user.Id, userSubscription.Id, plan.Id), ct);

        }, cancellationToken);

        return createdSubscriptionId;
    }
}
