

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public record EntitlementBalanceDto(
    string EntitlementKey,
    int DailyQuotaTotal,
    int UsedToday,
    int RemainingToday
);

public record EntitlementConsumeResult(
    bool Success, 
    string Message
);

public interface IEntitlementService
{
        Task<EntitlementConsumeResult> ConsumeAsync(EntitlementConsumeRequest request, CancellationToken ct);
    
        Task<EntitlementBalanceDto> GetBalanceAsync(
        Guid userId, 
        string entitlementKey, 
        CancellationToken ct);
    
        Task<List<EntitlementBalanceDto>> GetAllBalancesAsync(
        Guid userId, 
        CancellationToken ct);
}

public sealed record EntitlementConsumeRequest(
    Guid UserId,
    string EntitlementKey,
    string ReferenceSource,
    string ReferenceId,
    string IdempotencyKey);
