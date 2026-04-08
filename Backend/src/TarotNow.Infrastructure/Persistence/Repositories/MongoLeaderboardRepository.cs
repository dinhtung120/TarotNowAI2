using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính cho leaderboard, triển khai qua các partial theo nhóm nghiệp vụ.
public partial class MongoLeaderboardRepository : ILeaderboardRepository
{
    // Mongo context truy cập collections leaderboard entries/snapshots.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository leaderboard.
    /// Luồng xử lý: nhận MongoDbContext từ DI để dùng chung các thao tác score/rank/snapshot.
    /// </summary>
    public MongoLeaderboardRepository(MongoDbContext context)
    {
        _context = context;
    }
}
