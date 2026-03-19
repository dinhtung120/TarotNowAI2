/*
 * ===================================================================
 * FILE: MongoDbContext.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trung Tâm Điều Khiển Chống Xoay Náo MongoDB Ở Thằng C#. Thay Vì Truyền Sợi Dây MongoDB Trọc Cho Bọn Lính Service Đỡ Khóc. Mình Nẹp Vào Trong DBContext Xịn Đi Gọi Tắt Và Set Mọi TTL Điểm Xoáy Cho Nó Chuẩn Ở Ngay Code Này Sinh Init Khóa Sát Không Kẹt Code C#.
 * ===================================================================
 */

using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System;

namespace TarotNow.Infrastructure.Persistence;

/// <summary>
/// Máy Kéo Quản Lý Giao Thông Của Rổ MongoDB.
/// 
/// Tại Sao Éo Tiêm Cắm Thẳng Chữ `IMongoDatabase` Tẹt Vô Mấy Repo Luôn Cho Nhanh Khỏi Bọc Rác Gì Này?
/// 1. Gom Bọc Tụ: Code Ẩn Gọi Tên Cột Bảng, Khi Này Repo Thay Đổi Bảng Nào Cũng Tít Về Xử Lý Khóa Tránh Gõ Sai Text String Toàn App Dễ Văng (Magic String).
/// 2. Code Đi Trước Tạo Chỉ Mục Auto: Vừa Chạy Server Thì Gầm Mắt EnsureIndexes() Ở Đây Xảy Sống, Code Tự Tạo Dính Cột Index TTL Xóa Rác Tự Động Rút Nặng Thử Bảng (Chấp Đứt Init.js Quên Đánh Build Thằng Mongo Mảng Đốc Văng App Níu Lại).
/// 3. Viết Test Không Cháy Bảng Dễ Mock Ngang Khung ApplicationDbContext Của EF Core.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbContext> _logger;

    public MongoDbContext(IMongoDatabase database, ILogger<MongoDbContext> logger)
    {
        _database = database;
        _logger = logger;

        try
        {
            // Bão Táp Thổi Xé Bảng Nếu Cột Kéo DB Tụt Lệnh Quá To (Tạo Indexes Thạch Khóa SQL Báo Chặn Mạng)
            EnsureIndexes();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MongoDB] Failed to ensure indexes at startup.");
            throw; // Rớt DB Cứng Mõm Thì Cút Đi Code Dịch Chặn Đập Mái Khỏi Cho Web App Sống Do Bug Dính Lỗi Sai Database (Nhẫn Sai Database) Lỡ Tạo Mongo Nháp Sai Quá Thách App Kêu Ra Ráp Index Thất Bại Tụt Ra Trắng Báo Mẻ Khối Chứ Không Chạy Tiếp Ém Lỗi Rồi Cố Rác Database.
        }
    }

    // ======================================================================
    // 5 CÁC CON NHÓM THẠCH JSON (COLLECTIONS) NGÒI RÁC TRONG DB
    // ======================================================================

    /// <summary>Ổ 78 Con Giấy Bộ Bài Tarot Tĩnh — Ít Nhất Lùi Kéo Quét Nạp Thay Đổi Ở Giáp UI Nhét Nhóm Json.</summary>
    public IMongoCollection<CardCatalogDocument> Cards
        => _database.GetCollection<CardCatalogDocument>("cards_catalog");

    /// <summary>Đóng Gói Gọn Lỗ Gắn Lọc Sở Thú Gacha Bày Của User Ráp Nhặt Từng Nét Json To Lớn Giúp DB Ít Khớp.</summary>
    public IMongoCollection<UserCollectionDocument> UserCollections
        => _database.GetCollection<UserCollectionDocument>("user_collections");

    /// <summary>Trắng Session Hỏi Máy Giao Chat Rớt Toàn Bộ Cuộc Chuyển Về Mớ Lưu Bằng Mongo Tách Cả Ngàn Chữ Khỏi Postgre Tránh Mập Phanh.</summary>
    public IMongoCollection<ReadingSessionDocument> ReadingSessions
        => _database.GetCollection<ReadingSessionDocument>("reading_sessions");

    /// <summary>Toàn Văn Đoạn Trả Lời Raw Của Thằng OpenAI. Chữ Nặng To Nên Lưu Monggo TTL 90 Trả Quả Vứt Giữ Lỗi Report Để Re-Sync Prompt Error. Chứ Lưu EF Nghẽn Mở Lỗi Dump Báo Mực DB Chết Dump Đi Cả Postgre.</summary>
    public IMongoCollection<AiProviderLogDocument> AiProviderLogs
        => _database.GetCollection<AiProviderLogDocument>("ai_provider_logs");

    /// <summary>Gậy Notify Chuông Đẩy JSON Cho Các Sự Kiện. TTL Mất 30 Ngày Khỏe Kín Ngàn Quét Tránh Lag Lọc.</summary>
    public IMongoCollection<NotificationDocument> Notifications
        => _database.GetCollection<NotificationDocument>("notifications");

    /// <summary>Bảng Nhập CV Reader Thuộc Về SQL Khách Áp Gửi Rác File Rút Chữ Form App Rạch Gây Bể (Trốn Mongo).</summary>
    public IMongoCollection<ReaderRequestDocument> ReaderRequests
        => _database.GetCollection<ReaderRequestDocument>("reader_requests");

    /// <summary>Cáo Form Giới Thiệu Chào Khách Profile Của Thầy Bói Reader Giao Sang Ráp Document Cho Nhiều Bio.</summary>
    public IMongoCollection<ReaderProfileDocument> ReaderProfiles
        => _database.GetCollection<ReaderProfileDocument>("reader_profiles");

    /// <summary>Trụ Cái Hội Thoại Chat Giữa Client Khách Và Tâm Sĩ Tarot Chat Tí Quăng Vô Json Gộp Khúc Vĩ Đại Nhanh.</summary>
    public IMongoCollection<ConversationDocument> Conversations
        => _database.GetCollection<ConversationDocument>("conversations");

    /// <summary>Trống Nạp Bắn Nhanh Ngàn Cuốc Message Sót Trong Document Nhắn Náu. Chát NoSQL Tốc Độ Tốt.</summary>
    public IMongoCollection<ChatMessageDocument> ChatMessages
        => _database.GetCollection<ChatMessageDocument>("chat_messages");

    /// <summary>Report Cuốc Chat Láo Tránh Nuốt File Lỗi JSON Admin Đoạn</summary>
    public IMongoCollection<ReportDocument> Reports
        => _database.GetCollection<ReportDocument>("reports");

    // ======================================================================
    // MÁY CODE TRẤN CẮT QUY TẮC DB INDEX TTL HỒI PHỤC AUTO 
    // ======================================================================

    /// <summary>
    /// Tại Sao ÉP TẠO Trọc Lỗ Trong C# Không Nhét Init Giả DB Script Ngoài Docker?
    /// Vì Nhỡ Kỹ Sư Lôi Local Build Docker Không Có Init, Các Bảng Auto Bám Thiếu Index → App SQL Quét Full Mọi Lần Chat Rất Thối Cháy Ram Mongo Cực Trọng Máy Dev Giết Tới Database Rỗng Tịt Sạch Cụt App Ảm Đảm Crash Sáng Tận Production Mớ Index Không Cập Nhật Mà Quên Deploy File SQL Lạc Khét Lết Build Gọn Lưới Tại Đáy Class Báo Chặn Cháy App Lô Lớn Khẽ Giập Phát Cả Dải Giao Thông Nghe Chát SQL MongoDB Ló Nhanh Nhất (Code Là Tới Cháy Bền Chặt Tự Khỏe). Chết Index Thì Mỏ Cũng Kẹt Tự Gõ CreateOne Không Double Nhầm Nữa Nhát Mongo Nó Thông Minh Lọc Đi Index Duplicate Xả Rồi.
    /// </summary>
    private void EnsureIndexes()
    {
        // 1. Quát Chặn Bảng Tráo Thẻ (Cards Catalog) Cứa Khóa Unique Cấm Dễ Cắm Thêm Thẻ Mạo Danh Gây Crash Web Vít Bọn Tool Dịch.
        Cards.Indexes.CreateOne(new CreateIndexModel<CardCatalogDocument>(
            Builders<CardCatalogDocument>.IndexKeys.Ascending(c => c.Code),
            new CreateIndexOptions { Unique = true, Name = "idx_code_unique" }));

        // 2. Chặn Client Hack Đẻ Cây Có Tên Đôi Nhận Exp Đôi Khi Săn User Thạch.
        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys
                .Ascending(u => u.UserId)
                .Ascending(u => u.CardId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_cardid_unique" }));

        // Trích Trục Tìm Kiếm Kẹp Lọc Cho Kênh Quét Thẻ Nào Level Bét Khủng Lấy Làm Boss (Phục Vụ API Get Sớm Không Full Scan Nhanh Top Deck User).
        UserCollections.Indexes.CreateOne(new CreateIndexModel<UserCollectionDocument>(
            Builders<UserCollectionDocument>.IndexKeys
                .Ascending(u => u.UserId)
                .Descending(u => u.Level),
            new CreateIndexOptions { Name = "idx_userid_level_desc" }));

        // 3. Nạp Quét Gọi Nhanh Cụm Rạch Dứt Thảm History Session Bói App Phụ Ngát Theo Thời Gian.
        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        // Tìm Dọn Rác Thất Rớt AI Chat Sinh Đôi Cứt Rác Chết Nghẽn API Không Status "Completed" Đuổi Rễ.
        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.AiStatus)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_aistatus_createdat" }));

        ReadingSessions.Indexes.CreateOne(new CreateIndexModel<ReadingSessionDocument>(
            Builders<ReadingSessionDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Ascending(r => r.SpreadType)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_spreadtype_createdat" }));

        // 4. Mũ Hẹn Giờ TTL Nổ Rác AiProviderLogs Sau 90 Ngày Ngắn Không Chứa Tốn Storage Gây Vi Phạm Tiền Bạc (Tầm Tã Khổ Cắn RAM MongoDB Ngang). Lọc Rác.
        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys
                .Ascending(a => a.UserId)
                .Descending(a => a.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        AiProviderLogs.Indexes.CreateOne(new CreateIndexModel<AiProviderLogDocument>(
            Builders<AiProviderLogDocument>.IndexKeys.Ascending(a => a.CreatedAt),
            new CreateIndexOptions
            {
                Name = "idx_ttl_90d",
                ExpireAfter = TimeSpan.FromDays(90) // Auto-delete documents sau 90 ngày (Tính bằng Background Thread NoSQL Căn Ngày Báo Khắc Chết Gây Giải Phóng Dung Lưong Disk Về Cho Server Trắng Gọt).
            }));

        // 5. Mũ TTL Notification Cũng Ép Quát Cháy Nếu Dọn Giới 30 Ngày Khách Éo Đọc Gọi Kịp Trảm Luôn Rác Notif Quá 30D (Dung Lượng Tiết Kiệm App Nhẹ Client).
        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys
                .Ascending(n => n.UserId)
                .Ascending(n => n.IsRead)
                .Descending(n => n.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_isread_createdat" }));

        Notifications.Indexes.CreateOne(new CreateIndexModel<NotificationDocument>(
            Builders<NotificationDocument>.IndexKeys.Ascending(n => n.CreatedAt),
            new CreateIndexOptions
            {
                Name = "idx_ttl_30d",
                ExpireAfter = TimeSpan.FromDays(30) // Băm Gọt Tịt Chữ Báo Sau Ngậm Lọc Tháng Ko Quan Tâm Bay Màu Log 30s Database Nạp.
            }));

        // --- Reader_requests 
        ReaderRequests.Indexes.CreateOne(new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_userid_createdat_desc" }));

        // Trọc Bảng Đứng Soát Cho Admin Queue Soi Xét CV Pending.
        ReaderRequests.Indexes.CreateOne(new CreateIndexModel<ReaderRequestDocument>(
            Builders<ReaderRequestDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));

        // --- Cáo Chặn Khóa Reader_Profiles Ko Được Sở Hữu Kép 2 CV Khai Mạo DB X2 Nhũng.
        ReaderProfiles.Indexes.CreateOne(new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys.Ascending(r => r.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_userid_unique" }));

        // Tọng Cho Lên Danh Sách Dashboard App Tarot Show Active Online Nhanh Nhất (Quét Cục Nóng Status).
        ReaderProfiles.Indexes.CreateOne(new CreateIndexModel<ReaderProfileDocument>(
            Builders<ReaderProfileDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.UpdatedAt),
            new CreateIndexOptions { Name = "idx_status_updatedat_desc" }));

        // --- Cục Conversations Tạp Dây Liên Kết Khách Và Bói Lỗi Oa Rút Gắn 
        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.UserId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_userid_status_updatedat" }));

        Conversations.Indexes.CreateOne(new CreateIndexModel<ConversationDocument>(
            Builders<ConversationDocument>.IndexKeys
                .Ascending(c => c.ReaderId)
                .Ascending(c => c.Status)
                .Descending(c => c.UpdatedAt),
            new CreateIndexOptions { Name = "idx_readerid_status_updatedat" }));

        // --- Trùm Soát Chat Timeline Chống Mờ Phanh Chat Kéo Căng DB 
        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.ConversationId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_conversationid_createdat_desc" }));

        // Trích Ác Ai Độc Phát Spam Cần Soi DB Trace Report 
        ChatMessages.Indexes.CreateOne(new CreateIndexModel<ChatMessageDocument>(
            Builders<ChatMessageDocument>.IndexKeys
                .Ascending(m => m.SenderId)
                .Descending(m => m.CreatedAt),
            new CreateIndexOptions { Name = "idx_senderid_createdat_desc" }));

        // --- Băng Tạp Reports Giải Gọn Admin Đọc Nhanh Cuốc
        Reports.Indexes.CreateOne(new CreateIndexModel<ReportDocument>(
            Builders<ReportDocument>.IndexKeys
                .Ascending(r => r.Status)
                .Descending(r => r.CreatedAt),
            new CreateIndexOptions { Name = "idx_status_createdat_desc" }));
    }
}
