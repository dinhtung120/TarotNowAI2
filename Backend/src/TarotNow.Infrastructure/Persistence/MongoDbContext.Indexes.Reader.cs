using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
        private void EnsureReaderCollectionIndexes()
    {
        
        
        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        
        SafeCreateIndex(ReaderRequests, new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        
        
        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys.Ascending(r => r.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_unique" }));

        
        SafeCreateIndex(ReaderProfiles, new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys
                .Ascending(r => r.IsDeleted)
                .Ascending(r => r.Status)
                .Descending(r => r.UpdatedAt),
            new CreateIndexOptions { Name = "idx_isdeleted_status_updatedat_desc" }));
    }
}
