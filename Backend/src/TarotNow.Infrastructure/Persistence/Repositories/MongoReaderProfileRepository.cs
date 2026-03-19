/*
 * FILE: MongoReaderProfileRepository.cs
 * MỤC ĐÍCH: Repository quản lý hồ sơ Reader từ MongoDB (collection "reader_profiles").
 *
 *   CÁC CHỨC NĂNG:
 *   → AddAsync: tạo profile mới (khi Admin approve đơn xin làm Reader)
 *   → GetByUserIdAsync: lấy profile theo userId (unique index)
 *   → UpdateAsync: cập nhật (bio, pricing, status, v.v.)
 *   → DeleteByUserIdAsync: soft delete (set IsDeleted = true)
 *   → GetPaginatedAsync: trang danh sách Reader (directory) với bộ lọc nâng cao
 *
 *   DIRECTORY LISTING (GetPaginatedAsync):
 *   → Filter theo specialty (chuyên môn), status (online), searchTerm (tên)
 *   → Sắp xếp ưu tiên: accepting_questions > online > offline
 *   → Dùng MongoDB Aggregation Pipeline cho custom sort priority
 */

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IReaderProfileRepository — đọc/ghi hồ sơ Reader từ MongoDB.
/// Hỗ trợ directory listing với bộ lọc nâng cao và custom sort.
/// </summary>
public class MongoReaderProfileRepository : IReaderProfileRepository
{
    private readonly MongoDbContext _context;

    public MongoReaderProfileRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>Tạo profile mới, gán ObjectId về DTO.</summary>
    public async Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        await _context.ReaderProfiles.InsertOneAsync(doc, cancellationToken: cancellationToken);
        profile.Id = doc.Id;
    }

    /// <summary>
    /// Lấy profile theo userId. Unique index đảm bảo chỉ có 1 kết quả.
    /// Chỉ lấy chưa xóa (IsDeleted = false).
    /// </summary>
    public async Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderProfiles.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>Cập nhật profile: replace toàn bộ document + set UpdatedAt.</summary>
    public async Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderProfileDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderProfiles.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Soft delete profile theo userId — set IsDeleted = true + UpdatedAt.
    /// UpdateManyAsync: phòng trường hợp (hiếm) có nhiều profile cho 1 user.
    /// </summary>
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

    /// <summary>
    /// DIRECTORY LISTING — trang danh sách Reader với bộ lọc nâng cao.
    ///
    /// CÁC BỘ LỌC:
    /// → specialty: lọc theo chuyên môn (love, career, finance, v.v.)
    ///   Dùng AnyEq: kiểm tra array specialties CÓ CHỨA giá trị này không.
    /// → status: lọc theo trạng thái online (accepting_questions, online, offline)
    /// → searchTerm: tìm theo tên (case-insensitive regex)
    ///
    /// SẮP XẾP ĐẶC BIỆT (custom sort):
    /// → Ưu tiên: accepting_questions (0) > online (1) > offline (2) > unknown (3)
    /// → Trong cùng priority: updated_at mới nhất trước
    /// → Dùng MongoDB Aggregation Pipeline với $switch để tạo trường status_priority tạm thời.
    /// </summary>
    public async Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 12 : Math.Min(pageSize, 200);

        var filterBuilder = Builders<ReaderProfileDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        // Filter theo chuyên môn: array specialties chứa giá trị specialty
        if (!string.IsNullOrEmpty(specialty))
        {
            filter = filterBuilder.And(filter, filterBuilder.AnyEq(r => r.Specialties, specialty));
        }

        // Filter theo trạng thái online
        if (!string.IsNullOrEmpty(status))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, status));
        }

        // Tìm theo tên — case-insensitive regex ("i" flag)
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var regex = new MongoDB.Bson.BsonRegularExpression(searchTerm, "i");
            filter = filterBuilder.And(filter, filterBuilder.Regex(r => r.DisplayName, regex));
        }

        var totalCount = await _context.ReaderProfiles.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        // ==================== CUSTOM SORT ====================
        // Dùng Aggregation Pipeline thay vì Find().Sort() thông thường
        // vì cần sắp xếp theo PRIORITY tùy chỉnh (không phải giá trị string thường).
        //
        // $switch tạo trường tạm "status_priority":
        //   accepting_questions → 0 (ưu tiên cao nhất — Reader sẵn sàng nhận câu hỏi)
        //   online → 1 (đang online nhưng chưa sẵn sàng)
        //   offline → 2 (ngoại tuyến)
        //   mặc định → 3 (trạng thái không xác định)

        var statusPriority = new BsonDocument("$switch", new BsonDocument
        {
            {
                "branches", new BsonArray
                {
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.AcceptingQuestions }) },
                        { "then", 0 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Online }) },
                        { "then", 1 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Offline }) },
                        { "then", 2 }
                    }
                }
            },
            { "default", 3 }
        });

        // Aggregation Pipeline:
        // 1. $match: áp dụng filter
        // 2. $addFields: thêm trường tạm status_priority
        // 3. $sort: sắp xếp theo status_priority ASC (ưu tiên cao trước), updated_at DESC
        // 4. $skip/$limit: phân trang
        // 5. $project: xóa trường tạm status_priority trước khi trả về
        var docs = await _context.ReaderProfiles
            .Aggregate()
            .Match(filter)
            .AppendStage<BsonDocument>(new BsonDocument("$addFields", new BsonDocument("status_priority", statusPriority)))
            .AppendStage<BsonDocument>(new BsonDocument("$sort", new BsonDocument
            {
                { "status_priority", 1 },  // Ưu tiên cao trước
                { "updated_at", -1 }       // Cập nhật mới nhất trước
            }))
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .AppendStage<BsonDocument>(new BsonDocument("$project", new BsonDocument("status_priority", 0)))
            .As<ReaderProfileDocument>()
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    // ==================== MAPPING ====================

    /// <summary>Map Application DTO → MongoDB Document.</summary>
    private static ReaderProfileDocument ToDocument(ReaderProfileDto dto)
    {
        return new ReaderProfileDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            DisplayName = dto.DisplayName,
            AvatarUrl = dto.AvatarUrl,
            Pricing = new ReaderPricing { DiamondPerQuestion = dto.DiamondPerQuestion },
            Bio = new LocalizedText
            {
                Vi = dto.BioVi,
                En = dto.BioEn,
                Zh = dto.BioZh
            },
            Specialties = dto.Specialties,
            Stats = new ReaderStats
            {
                AvgRating = dto.AvgRating,
                TotalReviews = dto.TotalReviews
            },
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>Map MongoDB Document → Application DTO.</summary>
    private static ReaderProfileDto ToDto(ReaderProfileDocument doc)
    {
        return new ReaderProfileDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = doc.Status,
            DiamondPerQuestion = doc.Pricing.DiamondPerQuestion,
            BioVi = doc.Bio.Vi,
            BioEn = doc.Bio.En,
            BioZh = doc.Bio.Zh,
            Specialties = doc.Specialties,
            AvgRating = doc.Stats.AvgRating,
            TotalReviews = doc.Stats.TotalReviews,
            DisplayName = doc.DisplayName,
            AvatarUrl = doc.AvatarUrl,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
