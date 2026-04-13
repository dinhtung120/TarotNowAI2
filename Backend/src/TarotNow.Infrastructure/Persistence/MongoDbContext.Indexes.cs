namespace TarotNow.Infrastructure.Persistence;

// Điểm vào tổng để khởi tạo index cho toàn bộ Mongo collections.
public partial class MongoDbContext
{
    /// <summary>
    /// Điều phối tạo index cho mọi nhóm dữ liệu Mongo.
    /// Luồng xử lý: chạy lần lượt core, reader, chat, community, check-in, gamification và gacha.
    /// </summary>
    private void EnsureIndexes()
    {
        EnsureCoreCollectionIndexes();
        EnsureReaderCollectionIndexes();
        EnsureChatCollectionIndexes();
        EnsureCommunityIndexes();
        EnsureMediaUploadIndexes();
        EnsureCheckinIndexes();
        EnsureGamificationIndexes();
        EnsureGachaIndexes();
    }
}
