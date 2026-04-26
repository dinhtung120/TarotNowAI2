using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Helper phần side-effects billing và EXP cho reveal session.
/// </summary>
public sealed partial class ReadingSessionRevealRequestedDomainEventHandler
{
    private async Task<ReadingChargeSnapshot> ChargeReadingAsync(
        Guid userId,
        ReadingSession session,
        CancellationToken cancellationToken)
    {
        var amount = Math.Max(session.AmountCharged, 0);
        if (await TryConsumeFreeDrawAsync(userId, session, cancellationToken))
        {
            return default;
        }

        if (amount <= 0)
        {
            return default;
        }

        return await DebitReadingCostAsync(userId, session, amount, cancellationToken);
    }

    private async Task<bool> TryConsumeFreeDrawAsync(
        Guid userId,
        ReadingSession session,
        CancellationToken cancellationToken)
    {
        // Thẻ miễn phí lượt xem bài chỉ áp dụng khi user chọn thanh toán bằng gold.
        // Nếu user chọn diamond thì không được sử dụng free draw credit,
        // vì free draw ticket là phần thưởng dành riêng cho luồng gold.
        // Trước đây thiếu kiểm tra này khiến user ở tab diamond vẫn được miễn phí.
        if (string.Equals(session.CurrencyUsed, CurrencyType.Diamond, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var freeDrawSpreadCardCount = InventoryBusinessConstants.ResolveFreeDrawSpreadCardCount(session.SpreadType);
        if (freeDrawSpreadCardCount is null)
        {
            return false;
        }

        return await _freeDrawCreditRepository.TryConsumeAsync(
            userId,
            freeDrawSpreadCardCount.Value,
            cancellationToken);
    }

    private async Task<ReadingChargeSnapshot> DebitReadingCostAsync(
        Guid userId,
        ReadingSession session,
        long amount,
        CancellationToken cancellationToken)
    {
        var currency = NormalizeCurrency(session.CurrencyUsed);
        var changeType = ResolveReadingCostTransactionType(currency);

        try
        {
            var operationResult = await _walletRepository.DebitWithResultAsync(
                userId,
                currency,
                changeType,
                amount,
                "Reading",
                session.Id,
                $"Tarot reading reveal charge ({session.SpreadType})",
                metadataJson: null,
                idempotencyKey: $"reading_reveal_charge_{session.Id}",
                cancellationToken: cancellationToken);

            if (!operationResult.Executed)
            {
                return default;
            }

            await _domainEventPublisher.PublishAsync(
                new MoneyChangedDomainEvent
                {
                    UserId = userId,
                    Currency = currency,
                    ChangeType = changeType,
                    DeltaAmount = -amount,
                    ReferenceId = session.Id
                },
                cancellationToken);

            return new ReadingChargeSnapshot(
                Debited: true,
                Currency: currency,
                ChangeType: changeType,
                Amount: amount,
                ReferenceId: session.Id);
        }
        catch (InvalidOperationException)
        {
            throw new BadRequestException("Not enough balance to reveal this reading session.");
        }
    }

    private static string ResolveReadingCostTransactionType(string currency)
    {
        return currency == CurrencyType.Gold
            ? TransactionType.ReadingCostGold
            : TransactionType.ReadingCostDiamond;
    }

    private async Task ApplyCollectionStepAsync(
        Guid userId,
        ReadingSession session,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        var expToGrantPerCard = ResolveExpToGrant(session);
        foreach (var card in revealedCards)
        {
            await _userCollectionRepository.UpsertCardAsync(
                userId,
                card.CardId,
                expToGrantPerCard,
                card.Orientation,
                operationKey: BuildCardCollectionOperationKey(session.Id, card.CardId),
                cancellationToken: cancellationToken);
        }
    }

    private async Task GrantUserExpAsync(
        Guid userId,
        ReadingSession session,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        var expToGrantPerCard = ResolveExpToGrant(session);
        user.AddExp((long)(revealedCards.Count * expToGrantPerCard));
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    private static string BuildCardCollectionOperationKey(string sessionId, int cardId)
    {
        return $"reading_reveal_collection_{sessionId}_{cardId}";
    }

    private static string NormalizeCurrency(string? currency)
    {
        return string.Equals(currency, CurrencyType.Diamond, StringComparison.OrdinalIgnoreCase)
            ? CurrencyType.Diamond
            : CurrencyType.Gold;
    }

    private decimal ResolveExpToGrant(ReadingSession session)
    {
        var expPerCard = _systemConfigSettings.ProgressionReadingExpPerCard;
        var usesDiamond = string.Equals(
            session.CurrencyUsed,
            CurrencyType.Diamond,
            StringComparison.OrdinalIgnoreCase);
        var multiplier = _systemConfigSettings.ProgressionReadingDiamondMultiplierNonDaily;
        if (session.SpreadType != SpreadType.Daily1Card && usesDiamond)
        {
            return expPerCard * multiplier;
        }

        return expPerCard;
    }
}
