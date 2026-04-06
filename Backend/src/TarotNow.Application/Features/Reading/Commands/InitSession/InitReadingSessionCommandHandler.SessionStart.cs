using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public partial class InitReadingSessionCommandHandler
{
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

        if (!success) throw new BadRequestException("Failed to start session. Please try again.");
    }
}
