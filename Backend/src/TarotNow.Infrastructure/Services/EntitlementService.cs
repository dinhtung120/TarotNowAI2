/*
 * ===================================================================
 * FILE: EntitlementService.cs
 * NAMESPACE: TarotNow.Infrastructure.Services
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Máy Trọng Tài (Orchestrator) Điều Phối Giao Chăn Lệnh Đốt Và Đọc Số Dư Quyền Lợi.
 *   
 *   KIẾN TRÚC CAO CẤP:
 *   - Nó Không tự Đẩy SQL trực tiếp -> Nó Rót Lệnh Qua ISubscriptionRepository.
 *   - Nó Ôm Khóa Giao Dịch (ITransactionCoordinator) Để Bọc Atomic Lệnh Đốt (Tránh Trừ Khống Lệnh Database).
 *   - Nó Sẽ Phải Xài Redis (ICacheService) Để Tối Ưu Nhanh Read Lúc Chỉ Cần Ngía Hỏi Dư Bao Nhiêu.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

public class EntitlementService : IEntitlementService
{
    private readonly ISubscriptionRepository _repository;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly ICacheService _cacheService;
    private readonly ILogger<EntitlementService> _logger;
    private readonly IDomainEventPublisher _domainEventPublisher; // Để Bắn Sự Kiện Domain Ra Xa (RabbitMQ/Cache Nhận)

    public EntitlementService(
        ISubscriptionRepository repository,
        ITransactionCoordinator transactionCoordinator,
        ICacheService cacheService,
        ILogger<EntitlementService> logger,
        IDomainEventPublisher domainEventPublisher)
    {
        _repository = repository;
        _transactionCoordinator = transactionCoordinator;
        _cacheService = cacheService;
        _logger = logger;
        _domainEventPublisher = domainEventPublisher;
    }

    // ==========================================
    // 1. NGHIỆP VỤ RÚT THẺ NỘP VÀO
    // ==========================================
    public async Task<EntitlementConsumeResult> ConsumeAsync(
        Guid userId, 
        string entitlementKey, 
        string referenceSource, 
        string referenceId, 
        string idempotencyKey, 
        CancellationToken ct)
    {
        var result = new EntitlementConsumeResult(false, "Unknown error");
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);

        // Cuộn Vòng Bảo Kê Transaction. SQL Khóa Không Cho Thằng Đọc Kế Tiết Vô Quậy Nhầm.
        await _transactionCoordinator.ExecuteAsync(async (cancellation) =>
        {
            // [1] Check Trùng Trả Nhanh Kết Quả
            if (await _repository.ConsumeLogExistsAsync(idempotencyKey, cancellation))
            {
                _logger.LogInformation("Consume Idempotency Key {Key} already exists. Bỏ Qua Giao Dịch Lặp.", idempotencyKey);
                result = new EntitlementConsumeResult(true, "Already consumed (idempotent)");
                return;
            }

            // [2] Móc Lôi Ruột Ra Xem Rổ Đặc Quyền Của Hôm Nay, Giữ Chặt Tay ROW LOCK Lên Bảng
            var buckets = await _repository.GetBucketsForConsumeAsync(userId, entitlementKey, todayUtc, cancellation);

            // [3] Lọc & Bốc Giỏ Ra Ăn Từ Sớm Tới Hạn Kẹt Dưới Cùng.
            var targetBucket = buckets.FirstOrDefault(b => b.CanConsume(todayUtc));

            if (targetBucket != null)
            {
                // [4.A] Còn Quyền Trong Rổ Đặc Chủng. Trừ Ngay Lập Tức !
                targetBucket.Consume(todayUtc);

                var log = new EntitlementConsume(
                    userId: userId,
                    bucketId: targetBucket.Id,
                    entitlementKey: entitlementKey,
                    referenceSource: referenceSource,
                    referenceId: referenceId,
                    idempotencyKey: idempotencyKey
                );

                await _repository.AddConsumeLogAsync(log, cancellation);
                try
                {
                    await _repository.SaveChangesAsync(cancellation);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    _logger.LogError(ex, "[Entitlement] Save log failed. UserId: {UserId}, Key: {Key}, Ref: {RefId}, Idem: {Idem}. DB Error: {DbError}", 
                        userId, entitlementKey, referenceId, idempotencyKey, ex.InnerException?.Message);
                    throw;
                }

                // Publish Event Cho Thế Giới Biết Tao Vừa Đớp (Hủy Cache UI Update)
                await _domainEventPublisher.PublishAsync(new EntitlementConsumedDomainEvent(userId, entitlementKey, targetBucket.Id), cancellation);

                result = new EntitlementConsumeResult(true, "Consumed successfully");
                return;
            }

            // [4.B] Khách Bị Hết Sạch Cửa Khóa Gốc, Vậy Ta Có Thể Quy Đổi Luật Cho Phép Rớt Cấp Không (Cross-Mapping)?
            // Ví dụ: Bói Bộ 5 Lá Khách Xin Đu Bọn 3 Lá Què. (Rules Map)
            var mapRules = await _repository.GetEnabledMappingRulesAsync(entitlementKey, cancellation); // Source Là Cái Đang Xét Lỗi Này.
            if (mapRules.Any())
            {
                // Vét Cọc Nùi - Nới Sàng Mọi Rổ Để Chọn 1 Rổ Rơi Ưng Ý Nếu Có.
                // Tuy Nhiên Tạm Thời Hệ Thống Ưu Tiên Chưa Nhồi Tạp Găm Ràng Buộc Cao Đoạn Này Trừ Thẳng Để Tránh Lỗ Quản Trị Khó Fix.
                // Ở Hiện Tại: Mapping Default Là OFF Cấm Đào Sang Quy Đổi Lệch => Skip False Trả Diamond Đi Cháu!
                _logger.LogInformation("Tính Năng Cross Mapping Đang Xem Xét Đoạn Ratio 1:1. Tạm Thời Block Rơi Màng.");
            }

            // Mọi Chuyện Lạc Lỗi Rỗng Không
            result = new EntitlementConsumeResult(false, "No active quota available for this entitlement");
            
        }, ct);

        // Kẹp Sau Lộ SQL Gọi Xóa Chìa Redis Bọc Kỹ Sau Để Tránh Đụt Database Lại.
        if (result.Success)
        {
            var cacheKey = $"entitlement_balance:{userId}";
            await _cacheService.RemoveAsync(cacheKey); // Phá Quỹ Xem Ngang Này Đi Để User Bốc Hết Hồn Vào.
        }

        return result;
    }

    // ==========================================
    // 2. NGHIỆP VỤ LẤY SỔ SỐ DƯ
    // ==========================================
    public async Task<EntitlementBalanceDto> GetBalanceAsync(
        Guid userId, 
        string entitlementKey, 
        CancellationToken ct)
    {
        var allBalances = await GetAllBalancesAsync(userId, ct);
        return allBalances.FirstOrDefault(b => b.EntitlementKey == entitlementKey) 
               ?? new EntitlementBalanceDto(entitlementKey, 0, 0, 0);
    }

    public async Task<List<EntitlementBalanceDto>> GetAllBalancesAsync(
        Guid userId, 
        CancellationToken ct)
    {
        // [1] Xem Redis Đã Trả Nón Rồi Chưa Mới Nửa Tiếng?
        var cacheKey = $"entitlement_balance:{userId}";
        var cachedData = await _cacheService.GetAsync<string>(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedList = JsonSerializer.Deserialize<List<EntitlementBalanceDto>>(cachedData);
            if (cachedList != null) return cachedList;
        }

        // [2] Ồ DB Nghỉ Mát Kéo Lại Data 
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var balances = await _repository.GetBalanceSummaryAsync(userId, todayUtc, ct);

        // Lưu Cache
        await _cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(balances), TimeSpan.FromMinutes(10));
        return balances;
    }
}
