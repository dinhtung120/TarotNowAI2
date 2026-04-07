

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
    private readonly IDomainEventPublisher _domainEventPublisher; 

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
        
        var cacheKey = $"entitlement_balance:{userId}";
        var cachedData = await _cacheService.GetAsync<string>(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedList = JsonSerializer.Deserialize<List<EntitlementBalanceDto>>(cachedData);
            if (cachedList != null) return cachedList;
        }

        
        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var balances = await _repository.GetBalanceSummaryAsync(userId, todayUtc, ct);

        
        await _cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(balances), TimeSpan.FromMinutes(10));
        return balances;
    }
}
