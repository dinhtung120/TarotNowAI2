using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class GamificationService : IGamificationService
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
}
