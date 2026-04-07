

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;



public interface IGachaLogRepository
{
    Task InsertLogAsync(GachaLogInsertRequest request, CancellationToken ct);
    Task<List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>> GetUserLogsAsync(Guid userId, int limit, CancellationToken ct);
}

public sealed record GachaLogInsertRequest(
    Guid UserId,
    string BannerCode,
    string Rarity,
    string RewardType,
    string RewardValue,
    long SpentDiamond,
    bool WasPity,
    string? RngSeed,
    DateTime CreatedAt);
