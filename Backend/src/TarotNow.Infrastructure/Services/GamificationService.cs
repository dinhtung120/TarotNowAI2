using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Helpers;

namespace TarotNow.Infrastructure.Services;

public class GamificationService : IGamificationService
{
    private readonly IQuestRepository _questRepo;
    private readonly IAchievementRepository _achievementRepo;
    private readonly ILeaderboardRepository _leaderboardRepo;
    private readonly IGamificationPushService _pushService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GamificationService> _logger;

    public GamificationService(
        IQuestRepository questRepo,
        IAchievementRepository achievementRepo,
        ILeaderboardRepository leaderboardRepo,
        IGamificationPushService pushService,
        IMemoryCache cache,
        ILogger<GamificationService> logger)
    {
        _questRepo = questRepo;
        _achievementRepo = achievementRepo;
        _leaderboardRepo = leaderboardRepo;
        _pushService = pushService;
        _cache = cache;
        _logger = logger;
    }

    public async Task OnReadingCompletedAsync(Guid userId, CancellationToken ct)
    {
        try
        {
            // 1. Process Quests with trigger "reading_completed"
            // For simplicity, we assume we update daily progress for "daily_1_reading"
            string dailyPeriod = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string lifetimePeriod = "all";
            string monthlyPeriod = DateTime.UtcNow.ToString("yyyy-MM");

            // Hardcode or query active definitions by trigger event.
            // Optimized with IMemoryCache (Audit #6)
            var activeQuests = await _cache.GetOrCreateAsync("gamification_quests_all", entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return _questRepo.GetAllQuestsAsync(ct);
            });

            if (activeQuests == null) return;
            foreach(var q in activeQuests)
            {
                if (q.IsActive && q.TriggerEvent == "ReadingCompleted")
                {
                    string periodKey = PeriodKeyHelper.GetPeriodKey(q.QuestType);
                    await _questRepo.UpsertProgressAsync(userId, q.Code, periodKey, q.Target, 1, ct);
                }
            }

            // 2. Increment Leaderboard Scores
            // Earn EXP for reading: 10 points
            await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, 10, ct);
            await _leaderboardRepo.IncrementScoreAsync(userId, "monthly_rank_score", monthlyPeriod, 10, ct);
            await _leaderboardRepo.IncrementScoreAsync(userId, "lifetime_score", lifetimePeriod, 10, ct);

            // 3. Check achievements
            await CheckAndUnlockAchievementsAsync(userId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gamification error OnReadingCompleted for user {UserId}", userId);
        }
    }

    private async Task CheckAndUnlockAchievementsAsync(Guid userId, CancellationToken ct)
    {
        // 1. Get all definitions and user's current achievements
        var allDefs = await _achievementRepo.GetAllAchievementsAsync(ct);
        var unlocked = await _achievementRepo.GetUserAchievementsAsync(userId, ct);
        var unlockedCodes = unlocked.Select(u => u.AchievementCode).ToHashSet();

        foreach (var def in allDefs)
        {
            if (unlockedCodes.Contains(def.Code)) continue;

            // 2. Simple Rule Engine (Expandable)
            bool shouldUnlock = def.Code switch
            {
                "first_reading" => true, // Inherited from OnReadingCompleted call
                _ => false
            };

            if (shouldUnlock)
            {
                await _achievementRepo.UnlockAsync(userId, def.Code, ct);
                // Push real-time notification
                await _pushService.PushAchievementUnlockedAsync(userId, def.Code, def.GrantsTitleCode, ct);
            }
        }
    }

    public async Task OnCheckInAsync(Guid userId, int currentStreak, CancellationToken ct)
    {
        try
        {
            // Similar logic for check-in triggered quests
            var activeQuests = await _cache.GetOrCreateAsync("gamification_quests_all", entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return _questRepo.GetAllQuestsAsync(ct);
            });

            if (activeQuests == null) return;
            foreach (var q in activeQuests)
            {
                if (q.IsActive && q.TriggerEvent == "daily_checkin")
                {
                    string periodKey = PeriodKeyHelper.GetPeriodKey(q.QuestType);
                    await _questRepo.UpsertProgressAsync(userId, q.Code, periodKey, q.Target, 1, ct);
                }
                else if (q.IsActive && q.TriggerEvent == "streak_reached" && currentStreak >= q.Target)
                {
                    string periodKey = PeriodKeyHelper.GetPeriodKey(q.QuestType);
                    await _questRepo.UpsertProgressAsync(userId, q.Code, periodKey, q.Target, q.Target, ct);
                }
            }

            // 2. Increment Leaderboard Scores
            string dailyPeriod = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string monthlyPeriod = DateTime.UtcNow.ToString("yyyy-MM");
            string lifetimePeriod = "all";

            await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, 5, ct);
            await _leaderboardRepo.IncrementScoreAsync(userId, "monthly_rank_score", monthlyPeriod, 5, ct);
            await _leaderboardRepo.IncrementScoreAsync(userId, "lifetime_score", lifetimePeriod, 5, ct);
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Gamification error OnCheckIn for user {UserId}", userId);
        }
    }

    public async Task OnPostCreatedAsync(Guid userId, CancellationToken ct)
    {
        _logger.LogInformation("[Gamification] Xử lý sự kiện PostCreated cho User: {UserId}", userId);
        try
        {
            var activeQuests = await _cache.GetOrCreateAsync("gamification_quests_all", entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return _questRepo.GetAllQuestsAsync(ct);
            });

            if (activeQuests == null) return;
            foreach (var q in activeQuests)
            {
                if (q.IsActive && q.TriggerEvent == "PostCreated")
                {
                    string periodKey = PeriodKeyHelper.GetPeriodKey(q.QuestType);
                    await _questRepo.UpsertProgressAsync(userId, q.Code, periodKey, q.Target, 1, ct);
                    _logger.LogDebug("[Gamification] Cập nhật tiến độ nhiệm vụ {Code} cho User {UserId}", q.Code, userId);
                }
            }

            // Earn some small EXP for posting
            string dailyPeriod = DateTime.UtcNow.ToString("yyyy-MM-dd");
            await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, 2, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gamification error OnPostCreated for user {UserId}", userId);
        }
    }
}
