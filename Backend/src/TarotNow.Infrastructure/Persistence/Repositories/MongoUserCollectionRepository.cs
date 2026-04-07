

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoUserCollectionRepository : IUserCollectionRepository
{
    private readonly MongoDbContext _mongoContext;
    private readonly IRngService _rngService;

    public MongoUserCollectionRepository(MongoDbContext mongoContext, IRngService rngService)
    {
        _mongoContext = mongoContext;
        _rngService = rngService;
    }

        public async Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var filter = BuildUserCardFilter(userId, cardId);

        
        var existingDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(cancellationToken);
        int docBeforeUpsertLevel = existingDoc?.Level ?? 0;

        var update = BuildUpsertUpdate(userId, cardId, expToGain, now);

        var (updateResult, newDoc) = await UpsertAndGetDocAsync(filter, update, cancellationToken);
        
        
        
        
        if (updateResult.IsAcknowledged && newDoc != null && newDoc.Level > docBeforeUpsertLevel)
        {
            await ApplyRandomStatsOnLevelUpAsync(newDoc, docBeforeUpsertLevel, cancellationToken);
        }
    }

    private async Task<(UpdateResult, UserCollectionDocument?)> UpsertAndGetDocAsync(
        FilterDefinition<UserCollectionDocument> filter,
        PipelineUpdateDefinition<UserCollectionDocument> update,
        CancellationToken ct)
    {
        var result = await _mongoContext.UserCollections.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            ct);

        var newDoc = await _mongoContext.UserCollections.Find(filter).FirstOrDefaultAsync(ct);
        return (result, newDoc);
    }

    private async Task ApplyRandomStatsOnLevelUpAsync(UserCollectionDocument doc, int oldLevel, CancellationToken ct)
    {
        int levelsGained = doc.Level - oldLevel;
        if (levelsGained <= 0) return;

        int totalAtkBonus = 0;
        int totalDefBonus = 0;
        var rollRecords = new List<StatRollRecord>();

        
        for (int pLevel = oldLevel + 1; pLevel <= doc.Level; pLevel++)
        {
            var (min, max) = UserCollection.GetStatBonusRange(pLevel);
            
            
            
            int atkBonus = Random.Shared.Next(min, max + 1); 
            int defBonus = Random.Shared.Next(min, max + 1);

            totalAtkBonus += atkBonus;
            totalDefBonus += defBonus;

            rollRecords.Add(new StatRollRecord
            {
                Level = pLevel,
                AtkBonus = atkBonus,
                DefBonus = defBonus,
                RolledAt = DateTime.UtcNow
            });
        }

        
        var filter = Builders<UserCollectionDocument>.Filter.Eq(u => u.Id, doc.Id);
        var update = Builders<UserCollectionDocument>.Update
            .Inc(u => u.Atk, totalAtkBonus)
            .Inc(u => u.Def, totalDefBonus);

        if (doc.StatHistory == null)
        {
            update = update.Set(u => u.StatHistory, rollRecords);
        }
        else
        {
            update = update.PushEach(u => u.StatHistory, rollRecords);
        }

        await _mongoContext.UserCollections.UpdateOneAsync(filter, update, cancellationToken: ct);
    }

        public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var docs = await _mongoContext.UserCollections
            .Find(u => u.UserId == userIdStr && !u.IsDeleted)
            .SortByDescending(u => u.Level) 
            .ToListAsync(cancellationToken);

        return docs.Select(MapToEntity);
    }

}
