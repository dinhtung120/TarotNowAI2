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

// Repository quản lý định nghĩa title và title đã cấp cho user.
public class MongoTitleRepository : ITitleRepository
{
    // Mongo context truy cập collections titles và user_titles.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository title.
    /// Luồng xử lý: nhận MongoDbContext qua DI để thao tác dữ liệu title.
    /// </summary>
    public MongoTitleRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy toàn bộ title definitions.
    /// Luồng xử lý: query tất cả documents và map sang DTO.
    /// </summary>
    public async Task<List<TitleDefinitionDto>> GetAllTitlesAsync(CancellationToken ct)
    {
        var docs = await _context.Titles.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    /// <summary>
    /// Lấy title definition theo code.
    /// Luồng xử lý: truy vấn code duy nhất và map DTO nếu tồn tại.
    /// </summary>
    public async Task<TitleDefinitionDto?> GetByCodeAsync(string titleCode, CancellationToken ct)
    {
        var doc = await _context.Titles.Find(t => t.Code == titleCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

    /// <summary>
    /// Upsert title definition.
    /// Luồng xử lý: update theo code, set CreatedAt chỉ khi insert mới.
    /// </summary>
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
        // SetOnInsert giúp giữ nguyên thời điểm tạo ban đầu của title khi chỉnh sửa nhiều lần.

        await _context.Titles.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    /// <summary>
    /// Xóa title definition theo code.
    /// Luồng xử lý: delete một document khớp code.
    /// </summary>
    public async Task DeleteTitleDefinitionAsync(string titleCode, CancellationToken ct)
    {
        await _context.Titles.DeleteOneAsync(t => t.Code == titleCode, ct);
    }

    /// <summary>
    /// Lấy danh sách title user đã sở hữu.
    /// Luồng xử lý: filter theo userId và map về UserTitleDto.
    /// </summary>
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

    /// <summary>
    /// Kiểm tra user đã sở hữu title hay chưa.
    /// Luồng xử lý: count documents theo cặp userId-titleCode.
    /// </summary>
    public async Task<bool> OwnsTitleAsync(Guid userId, string titleCode, CancellationToken ct)
    {
        var filter = Builders<UserTitleDocument>.Filter.Eq(t => t.UserId, userId)
                   & Builders<UserTitleDocument>.Filter.Eq(t => t.TitleCode, titleCode);

        return await _context.UserTitles.CountDocumentsAsync(filter, cancellationToken: ct) > 0;
    }

    /// <summary>
    /// Cấp title cho user.
    /// Luồng xử lý: insert user-title mới; nếu trùng unique key thì bỏ qua như thao tác idempotent.
    /// </summary>
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
            // Edge case retry cấp title: bản ghi đã tồn tại nên không cần ném lỗi.
        }
    }

    /// <summary>
    /// Map title definition document sang DTO.
    /// Luồng xử lý: ánh xạ đầy đủ trường tên/mô tả/rarity/isActive.
    /// </summary>
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
