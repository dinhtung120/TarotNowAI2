
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service điều phối số dư và tiêu thụ entitlement cho người dùng.
public partial class EntitlementService : IEntitlementService
{
    // Repository subscription/bucket làm nguồn dữ liệu entitlement.
    private readonly ISubscriptionRepository _repository;
    // Coordinator transaction đảm bảo consume entitlement diễn ra atomically.
    private readonly ITransactionCoordinator _transactionCoordinator;
    // Cache giảm tải truy vấn số dư entitlement.
    private readonly ICacheService _cacheService;
    // Logger theo dõi nghiệp vụ entitlement.
    private readonly ILogger<EntitlementService> _logger;
    // Publisher phát domain event sau khi consume thành công.
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo service entitlement với đầy đủ phụ thuộc truy cập dữ liệu, cache và event.
    /// Luồng gom dependency ở constructor giúp các partial methods dùng nhất quán một ngữ cảnh xử lý.
    /// </summary>
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

    /// <summary>
    /// Lấy số dư của một entitlement cụ thể theo user.
    /// Luồng tái sử dụng danh sách số dư tổng để tránh lặp query khi cùng request cần nhiều key.
    /// </summary>
    public async Task<EntitlementBalanceDto> GetBalanceAsync(
        Guid userId,
        string entitlementKey,
        CancellationToken ct)
    {
        var allBalances = await GetAllBalancesAsync(userId, ct);
        // Nếu chưa có bucket cho key này thì trả DTO zero để contract API luôn ổn định.
        return allBalances.FirstOrDefault(b => b.EntitlementKey == entitlementKey)
               ?? new EntitlementBalanceDto(entitlementKey, 0, 0, 0);
    }

    /// <summary>
    /// Lấy toàn bộ số dư entitlement của user, ưu tiên đọc từ cache.
    /// Luồng cache-first giúp giảm tải database khi client gọi balance nhiều lần trong thời gian ngắn.
    /// </summary>
    public async Task<List<EntitlementBalanceDto>> GetAllBalancesAsync(
        Guid userId,
        CancellationToken ct)
    {
        // Dùng key cố định theo user để gom cache của toàn bộ entitlement keys.
        var cacheKey = $"entitlement_balance:{userId}";
        var cachedData = await _cacheService.GetAsync<string>(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedList = JsonSerializer.Deserialize<List<EntitlementBalanceDto>>(cachedData);
            if (cachedList != null)
            {
                // Cache parse được thì trả ngay để giảm độ trễ và số lần truy vấn DB.
                return cachedList;
            }
        }

        // Cache miss hoặc cache lỗi parse thì đọc lại từ repository theo ngày UTC hiện tại.
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var balances = await _repository.GetBalanceSummaryAsync(userId, todayUtc, ct);

        // Lưu cache 10 phút để cân bằng giữa tính mới và hiệu năng.
        await _cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(balances), TimeSpan.FromMinutes(10));
        return balances;
    }
}
