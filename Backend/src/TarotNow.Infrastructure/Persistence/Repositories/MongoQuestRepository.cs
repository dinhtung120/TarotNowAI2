using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính cho module quest, triển khai qua các partial theo nhóm thao tác.
public partial class MongoQuestRepository : IQuestRepository
{
    // Mongo context truy cập collections quests và quest_progress.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository quest.
    /// Luồng xử lý: nhận MongoDbContext từ DI để dùng thống nhất trong mọi thao tác định nghĩa/tiến độ/claim.
    /// </summary>
    public MongoQuestRepository(MongoDbContext context)
    {
        _context = context;
    }
}
