/*
 * ===================================================================
 * FILE: MongoDbContext.Indexes.Gacha.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence
 * ===================================================================
 */

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureGachaIndexes()
    {
        try
        {
            var logsCollection = GachaLogs;
            var indexKeysDefinition = Builders<GachaLogDocument>.IndexKeys
                .Descending(x => x.UserId)
                .Descending(x => x.CreatedAt);

            var indexModel = new CreateIndexModel<GachaLogDocument>(indexKeysDefinition, new CreateIndexOptions { Background = true });
            logsCollection.Indexes.CreateOne(indexModel);

            // TTL Index 30 Days
            var ttlIndexKeys = Builders<GachaLogDocument>.IndexKeys.Ascending(x => x.CreatedAt);
            var ttlOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(30), Background = true };
            var ttlIndexModel = new CreateIndexModel<GachaLogDocument>(ttlIndexKeys, ttlOptions);
            
            try 
            {
                logsCollection.Indexes.CreateOne(ttlIndexModel);
            }
            catch (MongoCommandException ex) when (ex.CodeName == "IndexOptionsConflict")
            {
                // Nếu IndexOptions (ví dụ ExpireAfter) bị thay đổi so với DB cũ, cần Drop và tạo lại
                logsCollection.Indexes.DropOne("created_at_1");
                logsCollection.Indexes.CreateOne(ttlIndexModel);
            }

            _logger.LogInformation("[MongoDB] Gacha Logs indexes created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Failed to create Gacha Logs indexes.");
        }
    }
}
