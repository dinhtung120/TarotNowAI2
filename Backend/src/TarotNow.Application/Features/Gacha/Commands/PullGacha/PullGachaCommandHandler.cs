using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.Features.Gacha.Commands.PullGacha;

/// <summary>
/// Handler pull gacha theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class PullGachaCommandHandler : IRequestHandler<PullGachaCommand, PullGachaResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo PullGachaCommandHandler.
    /// </summary>
    public PullGachaCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command pull gacha.
    /// </summary>
    public async Task<PullGachaResult> Handle(PullGachaCommand request, CancellationToken cancellationToken)
    {
        var normalizedPoolCode = NormalizePoolCode(request.PoolCode);
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);

        var domainEvent = new GachaPulledDomainEvent
        {
            UserId = request.UserId,
            PoolCode = normalizedPoolCode,
            Count = request.Count,
            IdempotencyKey = normalizedIdempotencyKey,
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);

        return new PullGachaResult
        {
            Success = true,
            IsIdempotentReplay = domainEvent.IsIdempotentReplay,
            OperationStatus = domainEvent.IsProcessingReplay
                ? PullGachaResult.OperationStatusProcessing
                : PullGachaResult.OperationStatusCompleted,
            OperationId = domainEvent.OperationId,
            PoolCode = domainEvent.PoolCode,
            CurrentPityCount = domainEvent.CurrentPityCount,
            HardPityThreshold = domainEvent.HardPityThreshold,
            WasPityTriggered = domainEvent.WasPityTriggered,
            Rewards = domainEvent.Rewards.Select(MapReward).ToList(),
        };
    }

    private static PullGachaRewardResult MapReward(GachaPullRewardSnapshot reward)
    {
        return new PullGachaRewardResult
        {
            Kind = reward.Kind,
            Rarity = reward.Rarity,
            Currency = reward.Currency,
            Amount = reward.Amount,
            ItemDefinitionId = reward.ItemDefinitionId,
            ItemCode = reward.ItemCode,
            QuantityGranted = reward.QuantityGranted,
            IconUrl = reward.IconUrl,
            NameVi = reward.NameVi,
            NameEn = reward.NameEn,
            NameZh = reward.NameZh,
        };
    }

    private static string NormalizePoolCode(string poolCode)
    {
        if (string.IsNullOrWhiteSpace(poolCode))
        {
            throw new BusinessRuleException(GachaErrorCodes.PoolNotFound, "Pool code is required.");
        }

        return poolCode.Trim().ToLowerInvariant();
    }

    private static string NormalizeIdempotencyKey(string idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BusinessRuleException(GachaErrorCodes.InvalidIdempotencyKey, "Idempotency key is required.");
        }

        return idempotencyKey.Trim();
    }
}
