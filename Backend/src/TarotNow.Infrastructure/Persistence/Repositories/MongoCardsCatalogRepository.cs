/*
 * FILE: MongoCardsCatalogRepository.cs
 * MỤC ĐÍCH: Repository đọc dữ liệu 78 lá bài Tarot từ MongoDB (collection "cards_catalog").
 *   Dữ liệu TĨNH (seed từ seed_cards.js) — chỉ có READ, không có Write.
 *
 *   CÁC CHỨC NĂNG:
 *   → GetByIdAsync: lấy 1 lá theo ID (0-77)
 *   → GetByCodeAsync: lấy 1 lá theo code ("the_fool", "ace_of_wands")
 *   → GetAllAsync: lấy toàn bộ 78 lá (cache startup hoặc random draw)
 *
 *   MAPPING: Document → DTO thủ công (không dùng AutoMapper vì quá đơn giản).
 */

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement ICardsCatalogRepository — đọc catalog 78 lá bài từ MongoDB.
/// Application layer chỉ nhận CardCatalogDto, không biết MongoDB Document tồn tại.
/// </summary>
public class MongoCardsCatalogRepository : ICardsCatalogRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoCardsCatalogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>Lấy 1 lá bài theo ID (0-77). Trả null nếu không tìm thấy.</summary>
    public async Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>
    /// Lấy 1 lá bài theo code (ví dụ: "the_fool"). Unique index đảm bảo chỉ có 1 kết quả.
    /// </summary>
    public async Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.Cards
            .Find(c => c.Code == code)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToDto(doc);
    }

    /// <summary>
    /// Lấy toàn bộ 78 lá, sắp xếp theo ID tăng dần.
    /// Dùng khi: khởi động app để cache, hoặc random draw cần biết tổng bộ bài.
    /// </summary>
    public async Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var docs = await _mongoContext.Cards
            .Find(_ => true)   // Lấy tất cả (không filter)
            .SortBy(c => c.Id) // Sắp xếp theo ID tăng dần (0 → 77)
            .ToListAsync(cancellationToken);

        return docs.Select(MapToDto);
    }

    /// <summary>
    /// Map MongoDB Document → Application DTO (thủ công, không dùng AutoMapper).
    /// Tại sao không dùng AutoMapper? 
    /// → Mapping quá đơn giản (chỉ copy property). AutoMapper gây overhead không cần thiết.
    /// → Tên cũng khác nhau (Name.Vi → NameVi) — cần config riêng nếu dùng AutoMapper.
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
        UprightDescription = doc.Meanings?.Upright?.Description ?? string.Empty,
        ReversedKeywords = doc.Meanings?.Reversed?.Keywords?.ToList() ?? new List<string>(),
        ReversedDescription = doc.Meanings?.Reversed?.Description ?? string.Empty
    };
}
