namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureIndexes()
    {
        EnsureCoreCollectionIndexes();
        EnsureReaderCollectionIndexes();
        EnsureChatCollectionIndexes();
        EnsureCommunityIndexes();
        EnsureCallIndexes();
    }
}
