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

        var (costGold, costDiamond) = request.SpreadType switch
        {
            SpreadType.Spread3Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread3GoldCost, _systemConfigSettings.Spread3DiamondCost),
            SpreadType.Spread5Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread5GoldCost, _systemConfigSettings.Spread5DiamondCost),
            SpreadType.Spread10Cards => GetConfiguredPrice(currency, _systemConfigSettings.Spread10GoldCost, _systemConfigSettings.Spread10DiamondCost),
            _ => (0L, 0L)
        };

        var amountCharged = costGold > 0 ? costGold : costDiamond;
        return new SessionPricing(costGold, costDiamond, currency, amountCharged);
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

    private static ReadingSession BuildSession(
        InitReadingSessionCommand request,
        string currencyUsed,
        long amountCharged)
    {
        return new ReadingSession(
            request.UserId.ToString(),
            request.SpreadType,
            request.Question,
            currencyUsed,
            amountCharged);
    }

    private async Task StartSessionAsync(
        InitReadingSessionCommand request,
        ReadingSession session,
        SessionPricing pricing,
        CancellationToken cancellationToken)
    {
        var (success, _) = await _readingSessionOrchestrator.StartPaidSessionAsync(
            new StartPaidSessionRequest
            {
                UserId = request.UserId,
                SpreadType = request.SpreadType,
                Session = session,
                CostGold = pricing.CostGold,
                CostDiamond = pricing.CostDiamond
            },
            cancellationToken);

        if (!success)
        {
            throw new BadRequestException("Failed to start session. Please try again.");
        }
    }
}
