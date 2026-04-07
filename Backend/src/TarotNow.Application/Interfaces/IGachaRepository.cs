

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IGachaRepository
{
    
    Task<GachaBanner?> GetActiveBannerAsync(string bannerCode, CancellationToken ct);
    Task<List<GachaBanner>> GetAllActiveBannersAsync(CancellationToken ct);
    Task<List<GachaBannerItem>> GetBannerItemsAsync(Guid bannerId, CancellationToken ct);
    
    
    Task<int> GetUserPityCountAsync(Guid userId, Guid bannerId, CancellationToken ct);
    
    
    Task<GachaRewardLog> LogRewardAsync(GachaRewardLog log, CancellationToken ct);
    Task<bool> IdempotencyKeyExistsAsync(string key, CancellationToken ct);
    
    
    Task<List<GachaRewardLog>> GetRewardLogsByIdempotencyKeyAsync(string key, CancellationToken ct);
}
