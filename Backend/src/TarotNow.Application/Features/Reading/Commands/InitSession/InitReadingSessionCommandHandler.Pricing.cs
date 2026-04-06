using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public partial class InitReadingSessionCommandHandler
{
    private readonly record struct SessionPricing(
        long CostGold,
        long CostDiamond,
        string CurrencyUsed,
        long AmountCharged);

    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
    }

    private async Task<SessionPricing> ResolvePricingAsync(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var currency = NormalizeCurrency(request.Currency);
        await EnsureDailyCardLimitAsync(request, cancellationToken);

        var (costGold, costDiamond) = ResolveBasePricing(request.SpreadType, currency);
        var entitlementKey = ResolveEntitlementKey(request.SpreadType);
        var usedEntitlement = await TryUseEntitlementAsync(request, entitlementKey, cancellationToken);
        if (usedEntitlement) (costGold, costDiamond) = (0, 0);

        var amountCharged = costGold > 0 ? costGold : costDiamond;
        return new SessionPricing(costGold, costDiamond, currency, amountCharged);
    }

    private (long CostGold, long CostDiamond) ResolveBasePricing(string spreadType, string currency)
    {
        return spreadType switch
        {
            SpreadType.Spread3Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread3GoldCost, _systemConfigSettings.Spread3DiamondCost),
            SpreadType.Spread5Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread5GoldCost, _systemConfigSettings.Spread5DiamondCost),
            SpreadType.Spread10Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread10GoldCost, _systemConfigSettings.Spread10DiamondCost),
            _ => (0L, 0L)
        };
    }

    private static string? ResolveEntitlementKey(string spreadType)
    {
        return spreadType switch
        {
            SpreadType.Spread3Cards => EntitlementKey.FreeSpread3Daily,
            SpreadType.Spread5Cards => EntitlementKey.FreeSpread5Daily,
            SpreadType.Spread10Cards => EntitlementKey.FreeSpread5Daily,
            _ => null
        };
    }

    private async Task<bool> TryUseEntitlementAsync(
        InitReadingSessionCommand request,
        string? entitlementKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(entitlementKey)) return false;
        var dateStr = DateTime.UtcNow.ToString("yyyyMMdd");
        var idempotencyKey = $"ir_{request.UserId:N}_{request.SpreadType}_{dateStr}_{Guid.NewGuid():N}";
        var consumeResult = await _entitlementService.ConsumeAsync(
            new EntitlementConsumeRequest(
                request.UserId,
                entitlementKey,
                "InitReadingSession",
                request.SpreadType.ToString(),
                idempotencyKey),
            cancellationToken);
        return consumeResult.Success;
    }

    private async Task EnsureDailyCardLimitAsync(
        InitReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        if (request.SpreadType != SpreadType.Daily1Card)
        {
            return;
        }

        var alreadyDrawn = await _readingRepo.HasDrawnDailyCardAsync(request.UserId, DateTime.UtcNow, cancellationToken);
        if (alreadyDrawn)
        {
            throw new BadRequestException("You have already drawn your free daily card today. Please try other spreads.");
        }
    }

    private static (long CostGold, long CostDiamond) GetConfiguredPrice(
        string currency,
        long goldPrice,
        long diamondPrice)
    {
        return currency == CurrencyType.Diamond
            ? (0, diamondPrice)
            : (goldPrice, 0);
    }

    private static string NormalizeCurrency(string? currency)
    {
        var normalized = currency?.Trim().ToLowerInvariant();
        return normalized == CurrencyType.Diamond ? CurrencyType.Diamond : CurrencyType.Gold;
    }

}
