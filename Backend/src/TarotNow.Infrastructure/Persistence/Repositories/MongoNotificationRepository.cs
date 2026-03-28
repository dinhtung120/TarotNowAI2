/*
 * FILE: MongoNotificationRepository.cs
 * MỤC ĐÍCH: Repository quản lý thông báo in-app từ MongoDB (collection "notifications").
 *   TTL 30 ngày — MongoDB tự xóa thông báo cũ.
 *
 *   CÁC CHỨC NĂNG:
 *   → CreateAsync: tạo thông báo mới (đa ngôn ngữ, metadata linh hoạt)
 *   → GetByUserIdAsync: lấy thông báo của User (filter đọc/chưa đọc, phân trang)
 *   → MarkAsReadAsync: đánh dấu 1 thông báo đã đọc (có kiểm tra ownership)
 *   → CountUnreadAsync: đếm thông báo chưa đọc (cho badge count trên UI)
 */

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement INotificationRepository — đọc/ghi thông báo từ MongoDB.
/// </summary>
public class MongoNotificationRepository : INotificationRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoNotificationRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Tạo thông báo mới cho User.
    /// Map từ DTO → Document: title/body đa ngôn ngữ, metadata dạng Dictionary → BsonDocument.
    /// Metadata linh hoạt: mỗi loại thông báo có metadata khác nhau (quest_code, deep_link, v.v.).
    /// </summary>
    public async Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default)
    {
        var doc = new NotificationDocument
        {
            UserId = notification.UserId.ToString(),
            Title = new LocalizedText
            {
                Vi = notification.TitleVi,
                En = notification.TitleEn,
                Zh = notification.TitleZh
            },
            Body = new LocalizedText
            {
                Vi = notification.BodyVi,
                En = notification.BodyEn,
                Zh = notification.BodyZh
            },
            Type = notification.Type,
            IsRead = false, // Thông báo mới luôn chưa đọc
            CreatedAt = DateTime.UtcNow
        };

        // Chuyển metadata Dictionary<string, string> → BsonDocument nếu có
        // BsonDocument cho phép lưu JSON tự do trong MongoDB
        if (notification.Metadata != null && notification.Metadata.Count > 0)
        {
            doc.Metadata = new BsonDocument(notification.Metadata
                .Select(kv => new BsonElement(kv.Key, kv.Value)));
        }

        await _mongoContext.Notifications.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy thông báo của User, hỗ trợ filter và phân trang.
    /// isRead = null → tất cả, true → đã đọc, false → chưa đọc.
    /// Sắp xếp: mới nhất trước (CreatedAt DESC).
    /// Trả về tuple: (danh sách DTO, tổng số) — UI dùng totalCount cho pagination.
    /// </summary>
    public async Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var userIdStr = userId.ToString();
        var filterBuilder = Builders<NotificationDocument>.Filter;

        // Filter cơ bản: đúng User
        var filter = filterBuilder.Eq(n => n.UserId, userIdStr);
        // Thêm filter isRead nếu có
        if (isRead.HasValue)
        {
            filter &= filterBuilder.Eq(n => n.IsRead, isRead.Value);
        }

        var totalCount = await _mongoContext.Notifications.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.Notifications
            .Find(filter)
            .SortByDescending(n => n.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        // Map Document → DTO
        var dtos = docs.Select(d => new NotificationDto
        {
            Id = d.Id,
            UserId = userId,
            TitleVi = d.Title.Vi,
            TitleEn = d.Title.En,
            BodyVi = d.Body.Vi,
            BodyEn = d.Body.En,
            Type = d.Type,
            IsRead = d.IsRead,
            CreatedAt = d.CreatedAt
        });

        return (dtos, totalCount);
    }

    /// <summary>
    /// Đánh dấu 1 thông báo đã đọc. Có kiểm tra OWNERSHIP:
    /// Filter bao gồm userId → User chỉ mark được thông báo CỦA MÌNH (không hack mark thông báo người khác).
    /// Trả về true nếu cập nhật thành công, false nếu không tìm thấy (sai ID hoặc không phải của User).
    /// </summary>
    public async Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var result = await _mongoContext.Notifications.UpdateOneAsync(
            n => n.Id == notificationId && n.UserId == userIdStr, // Ownership check
            Builders<NotificationDocument>.Update.Set(n => n.IsRead, true),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Đánh dấu tất cả thông báo TỒN TẠI VÀ CHƯA ĐỌC của User thành đã đọc.
    /// Dùng cho nút "Mark all as read". Rất tối ưu khi dùng UpdateMany thay vì loop.
    /// </summary>
    public async Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var result = await _mongoContext.Notifications.UpdateManyAsync(
            n => n.UserId == userIdStr && !n.IsRead,
            Builders<NotificationDocument>.Update.Set(n => n.IsRead, true),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Đếm số thông báo CHƯA ĐỌC của User — dùng cho badge count (số đỏ trên icon chuông).
    /// Query đơn giản: filter userId + IsRead = false → count.
    /// </summary>
    public async Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        return await _mongoContext.Notifications.CountDocumentsAsync(
            n => n.UserId == userIdStr && !n.IsRead,
            cancellationToken: cancellationToken);
    }
}
