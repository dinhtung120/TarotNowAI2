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
    private async Task ChargeReadingAsync(Guid userId, ReadingSession session, CancellationToken cancellationToken)
    {
        var amount = Math.Max(session.AmountCharged, 0);
        var currency = NormalizeCurrency(session.CurrencyUsed);
        if (amount <= 0)
        {
            return;
        }

        var changeType = currency == CurrencyType.Gold
            ? TransactionType.ReadingCostGold
            : TransactionType.ReadingCostDiamond;

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
                return;
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
        }
        catch (InvalidOperationException)
        {
            throw new BadRequestException("Not enough balance to reveal this reading session.");
        }
    }

    private async Task UpdateCollectionAndUserExpAsync(
        Guid userId,
        ReadingSession session,
        IReadOnlyList<ReadingDrawnCard> revealedCards,
        CancellationToken cancellationToken)
    {
        var expToGrantPerCard = ResolveExpToGrant(session) * ExpPerCard;
        foreach (var card in revealedCards)
        {
            await _userCollectionRepository.UpsertCardAsync(
                userId,
                card.CardId,
                expToGrantPerCard,
                card.Orientation,
                cancellationToken);
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        user.AddExp(revealedCards.Count * expToGrantPerCard);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    private static string NormalizeCurrency(string? currency)
    {
        return string.Equals(currency, CurrencyType.Diamond, StringComparison.OrdinalIgnoreCase)
            ? CurrencyType.Diamond
            : CurrencyType.Gold;
    }

    private static long ResolveExpToGrant(ReadingSession session)
    {
        var usesDiamond = string.Equals(
            session.CurrencyUsed,
            CurrencyType.Diamond,
            StringComparison.OrdinalIgnoreCase);

        return session.SpreadType != SpreadType.Daily1Card && usesDiamond ? 2 : 1;
    }
}
