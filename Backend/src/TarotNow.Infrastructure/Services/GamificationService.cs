using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service xử lý tiến trình gamification: quest, achievement và leaderboard.
public partial class GamificationService : IGamificationService
{
    // Repository quest phục vụ cập nhật tiến độ nhiệm vụ.
    private readonly IQuestRepository _questRepo;
    // Repository achievement phục vụ mở khóa thành tựu.
    private readonly IAchievementRepository _achievementRepo;
    // Repository leaderboard phục vụ cộng điểm xếp hạng.
    private readonly ILeaderboardRepository _leaderboardRepo;
    // Service push thông báo realtime khi có thành tựu/quest.
    private readonly IGamificationPushService _pushService;
    // Cache memory để giảm tần suất đọc danh sách quest định nghĩa.
    private readonly IMemoryCache _cache;
    // Logger cho truy vết lỗi trong luồng gamification.
    private readonly ILogger<GamificationService> _logger;

    /// <summary>
    /// Khởi tạo service gamification với đầy đủ phụ thuộc xử lý.
    /// Luồng này đảm bảo các partial methods dùng cùng một tập dependency nhất quán.
    /// </summary>
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
