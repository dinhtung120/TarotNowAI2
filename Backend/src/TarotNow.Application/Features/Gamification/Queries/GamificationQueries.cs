using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;
using Microsoft.Extensions.Logging;

namespace TarotNow.Application.Features.Gamification.Queries;

public record GetActiveQuestsQuery(Guid UserId, string QuestType) : IRequest<List<QuestWithProgressDto>>;

public class GetActiveQuestsQueryHandler : IRequestHandler<GetActiveQuestsQuery, List<QuestWithProgressDto>>
{
    private readonly IQuestRepository _questRepo;
    private readonly ILogger<GetActiveQuestsQueryHandler> _logger;

    public GetActiveQuestsQueryHandler(IQuestRepository questRepo, ILogger<GetActiveQuestsQueryHandler> logger)
    {
        _questRepo = questRepo;
        _logger = logger;
    }

    public async Task<List<QuestWithProgressDto>> Handle(GetActiveQuestsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Gamification] Lấy danh sách nhiệm vụ {Type} cho User: {UserId}", 
            request.QuestType, request.UserId);

        try 
        {
            // 1. Get definitions
            var activeQuests = await _questRepo.GetActiveQuestsAsync(request.QuestType, cancellationToken);
            _logger.LogDebug("[Gamification] Tìm thấy {Count} nhiệm vụ đang kích hoạt.", activeQuests.Count);
            
            // 2. Determine period key
            string periodKey = PeriodKeyHelper.GetPeriodKey(request.QuestType);
            _logger.LogDebug("[Gamification] Chu kỳ hiện tại: {PeriodKey}", periodKey);

            // 3. Get progress
            var currentProgress = await _questRepo.GetAllProgressAsync(request.UserId, request.QuestType, periodKey, cancellationToken);
            _logger.LogDebug("[Gamification] Tìm thấy {Count} bản ghi tiến độ.", currentProgress.Count);

            var result = new List<QuestWithProgressDto>();
            foreach (var def in activeQuests)
            {
                var prog = currentProgress.Find(p => p.QuestCode == def.Code);
                result.Add(new QuestWithProgressDto
                {
                    Definition = def,
                    Progress = prog
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Gamification] Lỗi khi lấy danh sách nhiệm vụ {Type} cho User: {UserId}", 
                request.QuestType, request.UserId);
            throw; // Re-throw to be caught by API Exception Middleware
        }
    }
}

public record GetUserAchievementsQuery(Guid UserId) : IRequest<UserAchievementsDto>;

public class GetUserAchievementsQueryHandler : IRequestHandler<GetUserAchievementsQuery, UserAchievementsDto>
{
    private readonly IAchievementRepository _achRepo;

    public GetUserAchievementsQueryHandler(IAchievementRepository achRepo)
    {
        _achRepo = achRepo;
    }

    public async Task<UserAchievementsDto> Handle(GetUserAchievementsQuery request, CancellationToken cancellationToken)
    {
        var definitions = await _achRepo.GetAllAchievementsAsync(cancellationToken);
        var unlocked = await _achRepo.GetUserAchievementsAsync(request.UserId, cancellationToken);

        return new UserAchievementsDto
        {
            Definitions = definitions,
            UnlockedList = unlocked
        };
    }
}

public record GetUserTitlesQuery(Guid UserId) : IRequest<UserTitlesDto>;

public class GetUserTitlesQueryHandler : IRequestHandler<GetUserTitlesQuery, UserTitlesDto>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    public GetUserTitlesQueryHandler(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

    public async Task<UserTitlesDto> Handle(GetUserTitlesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        var activeTitle = user?.ActiveTitleRef;

        var allTitles = await _titleRepo.GetAllTitlesAsync(cancellationToken);
        var userTitles = await _titleRepo.GetUserTitlesAsync(request.UserId, cancellationToken);
        
        var defDtos = new List<TitleDefinitionDto>();
        foreach(var t in allTitles) {
            defDtos.Add(new TitleDefinitionDto {
                Code = t.Code,
                NameVi = t.NameVi, NameEn = t.NameEn, NameZh = t.NameZh,
                DescriptionVi = t.DescriptionVi, DescriptionEn = t.DescriptionEn, DescriptionZh = t.DescriptionZh,
                Rarity = t.Rarity,
                IsActive = t.IsActive
            });
        }

        var unlockedDtos = new List<UserTitleDto>();
        foreach(var ut in userTitles) {
            unlockedDtos.Add(new UserTitleDto { TitleCode = ut.TitleCode, GrantedAt = ut.GrantedAt });
        }

        return new UserTitlesDto
        {
            Definitions = defDtos,
            UnlockedList = unlockedDtos,
            ActiveTitleCode = activeTitle
        };
    }
}

public record GetLeaderboardQuery(Guid? UserId, string ScoreTrack, string? PeriodKey, int Limit = 100) : IRequest<LeaderboardResultDto>;

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, LeaderboardResultDto>
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
        // 1. Chuẩn hóa ScoreTrack và PeriodKey
        string rawTrack = request.ScoreTrack?.Trim() ?? "spent_gold_daily";
        string scoreTrack = rawTrack.ToLowerInvariant();
        string? periodKey = request.PeriodKey?.Trim().ToLowerInvariant();

        // Nếu track có chứa hậu tố, ta tách chúng ra để làm sạch track và xác định kỳ hạn
        if (scoreTrack.EndsWith("_daily"))
        {
            scoreTrack = scoreTrack.Substring(0, scoreTrack.Length - 6);
            periodKey ??= PeriodKeyHelper.GetPeriodKey("daily");
        }
        else if (scoreTrack.EndsWith("_monthly"))
        {
            scoreTrack = scoreTrack.Substring(0, scoreTrack.Length - 8);
            periodKey ??= PeriodKeyHelper.GetPeriodKey("monthly");
        }
        else if (scoreTrack.EndsWith("_all"))
        {
            scoreTrack = scoreTrack.Substring(0, scoreTrack.Length - 4);
            periodKey ??= "all";
        }

        // Trường hợp đặc biệt nếu frontend gửi periodKey = "daily" thay vì track suffix
        if (periodKey == "daily") periodKey = PeriodKeyHelper.GetPeriodKey("daily");
        else if (periodKey == "monthly") periodKey = PeriodKeyHelper.GetPeriodKey("monthly");
        
        periodKey ??= "all";

        _logger.LogInformation("[Leaderboard] Query: {RawTrack} -> Normalized: {Track}-{Period}", 
            rawTrack, scoreTrack, periodKey);

        // 2. Lấy dữ liệu từ Snapshot (nếu có)
        var snapshot = await _lbRepo.GetSnapshotAsync(scoreTrack, periodKey, cancellationToken);
        LeaderboardResultDto result;

        if (snapshot != null)
        {
            _logger.LogInformation("[Leaderboard] Found snapshot for {Track}-{Period} with {Count} entries", scoreTrack, periodKey, snapshot.Entries.Count);
            result = new LeaderboardResultDto
            {
                Entries = snapshot.Entries,
                UserRank = null
            };
        }
        else
        {
            // 3. Lấy dữ liệu live từ DB
            var entries = await _lbRepo.GetTopEntriesAsync(scoreTrack, periodKey, request.Limit, cancellationToken);
            _logger.LogInformation("[Leaderboard] Found {Count} live entries for {Track}-{Period}", entries.Count, scoreTrack, periodKey);
            
            /*
             * [DIAGNOSTIC]: Kiểm tra xem dữ liệu thô từ MongoDB có tồn tại không.
             * Nếu Count > 0 nhưng UI vẫn trống, thì lỗi nằm ở phần Mapping thông tin User phía dưới.
             */
            if (entries.Count > 0)
            {
                _logger.LogInformation("[Leaderboard] Entry IDs: {Ids}", string.Join(", ", entries.Select(e => e.UserId)));
                
                var userIds = entries.Select(e => Guid.Parse(e.UserId)).Distinct();
                var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, cancellationToken);

                foreach (var entry in entries)
                {
                    if (userMap.TryGetValue(Guid.Parse(entry.UserId), out var info))
                    {
                        entry.DisplayName = info.DisplayName;
                        entry.Avatar = info.AvatarUrl;
                        entry.ActiveTitle = info.ActiveTitle;
                    }
                    else
                    {
                        _logger.LogWarning("[Leaderboard] No user info found in Postgres for UserId: {UserId}", entry.UserId);
                    }
                }
            }
            result = new LeaderboardResultDto { Entries = entries };
        }

        // 4. Lấy Rank của người dùng hiện tại
        if (request.UserId.HasValue)
        {
            var userRankEntry = await _lbRepo.GetUserRankAsync(request.UserId.Value, scoreTrack, periodKey, cancellationToken);
            if (userRankEntry != null)
            {
                var userInfo = await _userRepo.GetByIdAsync(request.UserId.Value, cancellationToken);
                result.UserRank = new UserRankDto
                {
                    ScoreTrack = scoreTrack,
                    PeriodKey = periodKey,
                    Rank = userRankEntry.Rank,
                    Score = userRankEntry.Score,
                    DisplayName = userInfo?.DisplayName ?? "Người dùng TarotNow",
                    Avatar = userInfo?.AvatarUrl,
                    ActiveTitle = userInfo?.ActiveTitleRef
                };
            }
        }

        return result;
    }
}
