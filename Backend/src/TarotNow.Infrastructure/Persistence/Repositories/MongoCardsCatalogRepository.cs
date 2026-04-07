

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class MongoCardsCatalogRepository : ICardsCatalogRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoCardsCatalogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

        public async Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

        public async Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Code == code)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

        public async Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var docs = await _mongoContext.Cards
            .Find(_ => true)   
            .SortBy(c => c.Id) 
            .ToListAsync(cancellationToken);

        return docs.Select(MapToDto);
    }

        private static CardCatalogDto MapToDto(CardCatalogDocument doc) => new()
    {
        Id = doc.Id,
        Code = doc.Code,
        NameVi = doc.Name.Vi,
        NameEn = doc.Name.En,
        NameZh = doc.Name.Zh,
        Arcana = doc.Arcana,
        Suit = doc.Suit,
        Element = doc.Element,
        Number = doc.Number,
        ImageUrl = doc.ImageUrl,
        UprightKeywords = doc.Meanings?.Upright?.Keywords?.ToList() ?? new List<string>(),
        UprightDescription = doc.Meanings?.Upright?.Description ?? string.Empty,
        ReversedKeywords = doc.Meanings?.Reversed?.Keywords?.ToList() ?? new List<string>(),
        ReversedDescription = doc.Meanings?.Reversed?.Description ?? string.Empty
    };
}
