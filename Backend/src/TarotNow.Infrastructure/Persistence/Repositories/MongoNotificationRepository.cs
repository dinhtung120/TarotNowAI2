

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý thông báo người dùng trên MongoDB.
public class MongoNotificationRepository : INotificationRepository
{
    // Mongo context truy cập collection notifications.
    private readonly MongoDbContext _mongoContext;

    /// <summary>
    /// Khởi tạo repository notification.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu thông báo.
    /// </summary>
    public MongoNotificationRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Tạo mới thông báo cho user.
    /// Luồng xử lý: map DTO sang document đa ngôn ngữ, gắn metadata tùy chọn rồi insert vào Mongo.
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
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        if (notification.Metadata != null && notification.Metadata.Count > 0)
        {
            doc.Metadata = new BsonDocument(notification.Metadata
                .Select(kv => new BsonElement(kv.Key, kv.Value)));
            // Chỉ gắn metadata khi có dữ liệu để giảm kích thước document lưu trữ.
        }

        await _mongoContext.Notifications.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy danh sách thông báo theo user có lọc trạng thái đọc và phân trang.
    /// Luồng xử lý: chuẩn hóa page/pageSize, dựng filter user + isRead tùy chọn, đếm tổng rồi lấy page mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Chặn page size quá lớn để bảo vệ API list notifications.

        var userIdStr = userId.ToString();
        var filterBuilder = Builders<NotificationDocument>.Filter;

        var filter = filterBuilder.Eq(n => n.UserId, userIdStr);

        if (isRead.HasValue)
        {
            filter &= filterBuilder.Eq(n => n.IsRead, isRead.Value);
            // Chỉ áp filter trạng thái đọc khi caller yêu cầu rõ ràng.
        }

        var totalCount = await _mongoContext.Notifications.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.Notifications
            .Find(filter)
            .SortByDescending(n => n.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

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
    /// Đánh dấu một thông báo đã đọc.
    /// Luồng xử lý: update theo notificationId + userId để tránh user tác động nhầm thông báo của người khác.
    /// </summary>
    public async Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var result = await _mongoContext.Notifications.UpdateOneAsync(
            n => n.Id == notificationId && n.UserId == userIdStr,
            Builders<NotificationDocument>.Update.Set(n => n.IsRead, true),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Đánh dấu toàn bộ thông báo chưa đọc của user thành đã đọc.
    /// Luồng xử lý: update many theo userId và điều kiện !isRead.
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
    /// Đếm số thông báo chưa đọc của user.
    /// Luồng xử lý: count documents theo userId và isRead=false.
    /// </summary>
    public async Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        return await _mongoContext.Notifications.CountDocumentsAsync(
            n => n.UserId == userIdStr && !n.IsRead,
            cancellationToken: cancellationToken);
    }
}
