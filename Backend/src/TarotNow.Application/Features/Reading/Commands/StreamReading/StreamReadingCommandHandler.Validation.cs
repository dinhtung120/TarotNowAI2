using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler
{
    private async Task<ReadingSession> ValidateSessionAsync(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        var session = await _readingRepo.GetByIdAsync(request.ReadingSessionId, cancellationToken);
        if (session == null)
        {
            throw new NotFoundException("Reading session not found");
        }

        if (session.UserId != request.UserId.ToString())
        {
            throw new UnauthorizedAccessException("Session not found or access denied");
        }

        if (!session.IsCompleted)
        {
            throw new BadRequestException("Cannot stream AI interpretation before revealing cards");
        }

        return session;
    }

    private async Task EnsureQuotaAsync(Guid userId, CancellationToken cancellationToken)
    {
        var dailyCount = await _aiRequestRepo.GetDailyAiRequestCountAsync(userId, cancellationToken);
        if (dailyCount >= _dailyAiQuota)
        {
            throw new BadRequestException("Daily AI request quota exceeded");
        }

        var activeCount = await _aiRequestRepo.GetActiveAiRequestCountAsync(userId, cancellationToken);
        if (activeCount >= _inFlightAiCap)
        {
            throw new BadRequestException("Too many in-flight AI requests");
        }
    }

    private async Task EnsureRateLimitAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rateLimitKey = $"ratelimit:{userId}:ai_interpret";
        var rateLimit = TimeSpan.FromSeconds(_readingRateLimitSeconds);
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, rateLimit, cancellationToken);
        if (isAllowed)
        {
            return;
        }

        throw new BadRequestException($"Vui lòng đợi {_readingRateLimitSeconds} giây giữa các lần yêu cầu AI giải bài.");
    }
}
