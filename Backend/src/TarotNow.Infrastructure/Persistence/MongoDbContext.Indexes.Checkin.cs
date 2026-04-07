using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCheckinIndexes()
    {
        
        
        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.UserId).Ascending(n => n.BusinessDate),
            new CreateIndexOptions { Name = "idx_userid_businessdate_unique", Unique = true }));

        
        
        SafeCreateIndex(DailyCheckins, new CreateIndexModel<DailyCheckinDocument>(
            Builders<DailyCheckinDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));
    }
}
