using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

public record GetLeaderboardQuery(Guid? UserId, string ScoreTrack, string? PeriodKey, int Limit = 100) : IRequest<LeaderboardResultDto>;

public partial class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, LeaderboardResultDto>
{
    private readonly ILeaderboardRepository _lbRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<GetLeaderboardQueryHandler> _logger;

    public GetLeaderboardQueryHandler(
        ILeaderboardRepository lbRepo,
        IUserRepository userRepo,
        ILogger<GetLeaderboardQueryHandler> logger)
    {
        _lbRepo = lbRepo;
        _userRepo = userRepo;
        _logger = logger;
    }

    public async Task<LeaderboardResultDto> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var normalized = NormalizeTrack(request.ScoreTrack, request.PeriodKey);
        _logger.LogInformation("[Leaderboard] Query: {RawTrack} -> Normalized: {Track}-{Period}", request.ScoreTrack, normalized.Track, normalized.PeriodKey);
        var result = await LoadLeaderboardAsync(normalized, request.Limit, cancellationToken);
        if (request.UserId.HasValue)
        {
            await PopulateUserRankAsync(result, request.UserId.Value, normalized, cancellationToken);
        }

        return result;
    }
}
