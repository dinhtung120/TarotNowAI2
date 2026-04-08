using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository chính quản lý hồ sơ reader trên Mongo.
public partial class MongoReaderProfileRepository : IReaderProfileRepository
{
    // Mongo context truy cập collection reader_profiles.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository reader profile.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu profile.
    /// </summary>
    public MongoReaderProfileRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới reader profile.
    /// Luồng xử lý: map DTO sang document, insert Mongo và trả lại id phát sinh qua đối tượng DTO.
    /// </summary>
    public async Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        await _context.ReaderProfiles.InsertOneAsync(doc, cancellationToken: cancellationToken);
        profile.Id = doc.Id;
    }

    /// <summary>
    /// Lấy reader profile theo userId.
    /// Luồng xử lý: filter userId + is_deleted=false rồi map về DTO.
    /// </summary>
    public async Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false));

        var doc = await _context.ReaderProfiles.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy danh sách reader profile theo nhiều userId.
    /// Luồng xử lý: filter In(userIds) + is_deleted=false và map DTO.
    /// </summary>
    public async Task<IEnumerable<ReaderProfileDto>> GetByUserIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderProfileDocument>.Filter.And(
            Builders<ReaderProfileDocument>.Filter.In(r => r.UserId, userIds),
            Builders<ReaderProfileDocument>.Filter.Eq(r => r.IsDeleted, false));

        var docs = await _context.ReaderProfiles.Find(filter).ToListAsync(cancellationToken);
        return docs.Select(ToDto);
    }

    /// <summary>
    /// Cập nhật toàn bộ reader profile.
    /// Luồng xử lý: replace document theo id và set UpdatedAt hiện tại.
    /// </summary>
    public async Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(profile);
        doc.UpdatedAt = DateTime.UtcNow;
        // Ghi mốc cập nhật mới để hỗ trợ sort directory theo updated_at.

        var filter = Builders<ReaderProfileDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderProfiles.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Xóa mềm profile theo userId.
    /// Luồng xử lý: update nhiều bản ghi khớp userId chưa xóa, set is_deleted=true và updated_at.
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
}
