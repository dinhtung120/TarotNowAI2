using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReaderProfileRepository : IReaderProfileRepository
{
    private readonly MongoDbContext _context;

    public MongoReaderProfileRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        await _context.ReaderProfiles.InsertOneAsync(doc, cancellationToken: cancellationToken);
        profile.Id = doc.Id;
    }

    public async Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false));

        var doc = await _context.ReaderProfiles.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    public async Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderProfileDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderProfiles.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    public async Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false));

        var update = Builders<ReaderProfileDocument>.Update
            .Set(r => r.IsDeleted, true)
            .Set(r => r.UpdatedAt, DateTime.UtcNow);

        await _context.ReaderProfiles.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }
}
