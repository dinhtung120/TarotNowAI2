using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class MongoTitleRepository : ITitleRepository
{
    private readonly MongoDbContext _context;

    public MongoTitleRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<TitleDefinitionDto>> GetAllTitlesAsync(CancellationToken ct)
    {
        var docs = await _context.Titles.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    public async Task<TitleDefinitionDto?> GetByCodeAsync(string titleCode, CancellationToken ct)
    {
        var doc = await _context.Titles.Find(t => t.Code == titleCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

    public async Task UpsertTitleDefinitionAsync(TitleDefinitionDto title, CancellationToken ct)
    {
        var filter = Builders<TitleDefinitionDocument>.Filter.Eq(t => t.Code, title.Code);
        var update = Builders<TitleDefinitionDocument>.Update
            .Set(t => t.NameVi, title.NameVi)
            .Set(t => t.NameEn, title.NameEn)
            .Set(t => t.NameZh, title.NameZh)
            .Set(t => t.DescriptionVi, title.DescriptionVi)
            .Set(t => t.DescriptionEn, title.DescriptionEn)
            .Set(t => t.DescriptionZh, title.DescriptionZh)
            .Set(t => t.Rarity, title.Rarity)
            .Set(t => t.IsActive, title.IsActive)
            .SetOnInsert(t => t.CreatedAt, DateTime.UtcNow);

        await _context.Titles.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task DeleteTitleDefinitionAsync(string titleCode, CancellationToken ct)
    {
        await _context.Titles.DeleteOneAsync(t => t.Code == titleCode, ct);
    }

    public async Task<List<UserTitleDto>> GetUserTitlesAsync(Guid userId, CancellationToken ct)
    {
        var filter = Builders<UserTitleDocument>.Filter.Eq(t => t.UserId, userId);
        var docs = await _context.UserTitles.Find(filter).ToListAsync(ct);

        return docs.Select(d => new UserTitleDto
        {
            TitleCode = d.TitleCode,
            GrantedAt = d.GrantedAt
        }).ToList();
    }

    public async Task<bool> OwnsTitleAsync(Guid userId, string titleCode, CancellationToken ct)
    {
        var filter = Builders<UserTitleDocument>.Filter.Eq(t => t.UserId, userId)
                   & Builders<UserTitleDocument>.Filter.Eq(t => t.TitleCode, titleCode);
        
        return await _context.UserTitles.CountDocumentsAsync(filter, cancellationToken: ct) > 0;
    }

    public async Task GrantTitleAsync(Guid userId, string titleCode, CancellationToken ct)
    {
        try
        {
            var doc = new UserTitleDocument
            {
                UserId = userId,
                TitleCode = titleCode,
                GrantedAt = DateTime.UtcNow
            };
            await _context.UserTitles.InsertOneAsync(doc, cancellationToken: ct);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Idempotency: Ignore duplicate key error if user already owns this title
        }
    }

    private TitleDefinitionDto MapDefinition(TitleDefinitionDocument doc)
    {
        return new TitleDefinitionDto
        {
            Code = doc.Code,
            NameVi = doc.NameVi,
            NameEn = doc.NameEn,
            NameZh = doc.NameZh,
            DescriptionVi = doc.DescriptionVi,
            DescriptionEn = doc.DescriptionEn,
            DescriptionZh = doc.DescriptionZh,
            Rarity = doc.Rarity,
            IsActive = doc.IsActive
        };
    }
}
