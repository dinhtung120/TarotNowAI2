using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation cho ICardsCatalogRepository.
///
/// Đọc dữ liệu 78 lá bài Tarot từ collection "cards_catalog".
/// Dữ liệu tĩnh (seed từ seed_cards.js), chỉ có READ operations.
/// 
/// Mapping: Document → DTO diễn ra tại repository level,
/// đảm bảo Application layer không biết MongoDB tồn tại.
/// </summary>
public class MongoCardsCatalogRepository : ICardsCatalogRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoCardsCatalogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>Lấy 1 lá bài theo ID (0-77).</summary>
    public async Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>Lấy 1 lá bài theo code (VD: "the_fool") — dùng unique index.</summary>
    public async Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Code == code)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>Lấy toàn bộ 78 lá — dùng cho cache startup hoặc random draw.</summary>
    public async Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var docs = await _mongoContext.Cards
            .Find(_ => true)
            .SortBy(c => c.Id)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToDto);
    }

    /// <summary>
    /// Map MongoDB document → Application DTO.
    /// Tại sao không dùng AutoMapper? → Quá đơn giản, không cần overhead của AutoMapper.
    /// </summary>
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
        Number = doc.Number
    };
}
