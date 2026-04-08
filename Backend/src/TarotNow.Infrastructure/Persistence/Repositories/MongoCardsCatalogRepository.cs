

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository truy vấn danh mục lá bài tarot từ Mongo.
public class MongoCardsCatalogRepository : ICardsCatalogRepository
{
    // Mongo context truy cập collection cards_catalog.
    private readonly MongoDbContext _mongoContext;

    /// <summary>
    /// Khởi tạo repository cards catalog.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu danh mục.
    /// </summary>
    public MongoCardsCatalogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Lấy thông tin card theo id số nguyên.
    /// Luồng xử lý: query collection theo id và map sang DTO nếu tồn tại.
    /// </summary>
    public async Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>
    /// Lấy thông tin card theo mã code.
    /// Luồng xử lý: query theo code và map sang DTO.
    /// </summary>
    public async Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Code == code)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>
    /// Lấy toàn bộ danh mục card.
    /// Luồng xử lý: lấy tất cả documents, sắp theo id tăng dần để giữ thứ tự ổn định rồi map DTO.
    /// </summary>
    public async Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var docs = await _mongoContext.Cards
            .Find(_ => true)
            .SortBy(c => c.Id)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToDto);
    }

    /// <summary>
    /// Map document card sang DTO dùng cho tầng Application.
    /// Luồng xử lý: ánh xạ đầy đủ thông tin đa ngôn ngữ và fallback list rỗng khi thiếu meaning keywords.
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
        Number = doc.Number,
        ImageUrl = doc.ImageUrl,
        UprightKeywords = doc.Meanings?.Upright?.Keywords?.ToList() ?? new List<string>(),
        // Edge case: dữ liệu seed cũ có thể thiếu meanings, fallback để không null ở response.
        UprightDescription = doc.Meanings?.Upright?.Description ?? string.Empty,
        ReversedKeywords = doc.Meanings?.Reversed?.Keywords?.ToList() ?? new List<string>(),
        ReversedDescription = doc.Meanings?.Reversed?.Description ?? string.Empty
    };
}
