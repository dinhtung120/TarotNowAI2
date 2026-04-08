using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Queries;

// Query lấy bảng xếp hạng theo track/period.
public record GetLeaderboardQuery(Guid? UserId, string ScoreTrack, string? PeriodKey, int Limit = 100) : IRequest<LeaderboardResultDto>;

// Handler truy vấn leaderboard.
public partial class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, LeaderboardResultDto>
{
    private readonly ILeaderboardRepository _lbRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<GetLeaderboardQueryHandler> _logger;

    /// <summary>
    /// Khởi tạo handler get leaderboard.
    /// Luồng xử lý: nhận leaderboard repository, user repository và logger để tải dữ liệu top + user rank.
    /// </summary>
    public GetLeaderboardQueryHandler(
        ILeaderboardRepository lbRepo,
        IUserRepository userRepo,
        ILogger<GetLeaderboardQueryHandler> logger)
    {
        _lbRepo = lbRepo;
        _userRepo = userRepo;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý query lấy leaderboard.
    /// Luồng xử lý: normalize track/period, tải top leaderboard, nếu có UserId thì enrich thêm hạng cá nhân.
    /// </summary>
    public async Task<LeaderboardResultDto> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var normalized = NormalizeTrack(request.ScoreTrack, request.PeriodKey);
        _logger.LogInformation("[Leaderboard] Query: {RawTrack} -> Normalized: {Track}-{Period}", request.ScoreTrack, normalized.Track, normalized.PeriodKey);
        var result = await LoadLeaderboardAsync(normalized, request.Limit, cancellationToken);
        if (request.UserId.HasValue)
        {
            // Chỉ tính user rank khi caller truyền UserId.
            await PopulateUserRankAsync(result, request.UserId.Value, normalized, cancellationToken);
        }

        return result;
    }
}
