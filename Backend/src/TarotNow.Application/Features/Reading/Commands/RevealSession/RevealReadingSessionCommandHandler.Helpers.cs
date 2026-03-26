using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public partial class RevealReadingSessionCommandHandler
{
    private async Task<ReadingSession> GetSessionForRevealAsync(
        RevealReadingSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException("Session not found");

        if (session.UserId != request.UserId.ToString())
        {
            throw new UnauthorizedAccessException("Reading session not found or access denied");
        }

        if (session.IsCompleted)
        {
            throw new BadRequestException("This session has already been revealed");
        }

        return session;
    }

    private static int ResolveCardsToDraw(string spreadType)
    {
        return spreadType switch
        {
            SpreadType.Daily1Card => 1,
            SpreadType.Spread3Cards => 3,
            SpreadType.Spread5Cards => 5,
            SpreadType.Spread10Cards => 10,
            _ => throw new BadRequestException($"Invalid spread type: {spreadType}")
        };
    }

    private static long ResolveExpToGrant(ReadingSession session)
    {
        var usesDiamond = string.Equals(
            session.CurrencyUsed,
            CurrencyType.Diamond,
            StringComparison.OrdinalIgnoreCase);

        return session.SpreadType != SpreadType.Daily1Card && usesDiamond ? 2 : 1;
    }

    private async Task UpdateCollectionAsync(
        Guid userId,
        IEnumerable<int> drawnCards,
        long expToGrant,
        CancellationToken cancellationToken)
    {
        foreach (var cardId in drawnCards)
        {
            await _collectionRepo.UpsertCardAsync(userId, cardId, expToGrant, cancellationToken);
        }
    }

    private async Task ApplyUserExpAsync(
        Guid userId,
        long expAmount,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return;
        }

        user.AddExp(expAmount);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}
