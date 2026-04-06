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
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class EntitlementService : IEntitlementService
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
