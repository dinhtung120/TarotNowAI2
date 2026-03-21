/*
 * ===================================================================
 * FILE: ReadingSession.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Đóng Kén Một Lượt Rút Bài (ReadingSession).
 *   Sử Dụng Ở SQL Để Căn Xem Phiên Chơi Này Được Rút Ngày Nào Tốn Xu Nào.
 * ===================================================================
 */

using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho một phiên Khách Đập Bàn Hét Đòi Rút Lá Tarot.
/// Lưu ý: Bản ghi này ở SQL Postgres - Tập trung theo dõi Lượng Tiền Trừ (AmountCharged) 
/// Phân mảnh MongoDB Mới Là Đứa Lưu Trữ Lời Giải Thích Dài Cuộn Theo.
/// </summary>
public class ReadingSession
{
    // Bút Mã Định Danh Của Dòng Phiên Bói Tán.
    public string Id { get; private set; } = string.Empty;
    // Khách Hàng Nào Chơi Cờ Bạc Phiên Này (Trỏ Lên GUID User).
    public string UserId { get; private set; } = string.Empty;
    
    // Loại Lịch Bày Bố Quẻ Tarot Nổi Tiếng Nào Đang Xài (Cross, Daily Rút Mảnh, Love 3 Lá...)
    public string SpreadType { get; private set; } = string.Empty;

    /// <summary>
    /// Gửi Thông Điệp Ước Nguyện Gì Của Khách Muốn Thay Đổi Tới Vũ Trụ Trong Đầu Lúc Rút.
    /// Phase 1.3: "Giờ em thèm lấy thằng đại gia kia làm Chồng Nhờ Cô Giải Dùm Lá" (Question Tùy Chọn).
    /// </summary>
    public string? Question { get; private set; }

    // Kho Mảng Array Text: Mấy Lá Nào Được Chọt Nằm Ở Slot Nào Đựng Bỏ Vô Mảng (VD Rút Được Lá Mã [20, 11, 00]).
    public string? CardsDrawn { get; private set; }

    /// <summary>
    /// Đã Dùng Kim Cương Hay Vàng Để Ném Trả (Ai Nạp Đô Dùng Kim Cương Sang Xịn Trừ Đi Có Cấp EXP Gấp Bội Hơn Môn Đồ Chơi Free Cày Vàng Ra Từ Nhiệm Vụ Quảng Cáo).
    /// </summary>
    public string? CurrencyUsed { get; private set; }

    /// <summary>
    /// Bọc Số Liền Chốt Giá Bơm Phí Xong - Trừ 5 Hoặc 10 Nhát Mất Vĩnh Viễn Túi Ví Khách.
    /// </summary>
    public long AmountCharged { get; private set; }

    // Đánh Dấu Phiên Giao Cảm Bài Này Đã Đọc Dịch Thấy Ánh Sáng Ra Text AI Thành Công Hay Mới Chỉ Treo Trừ Tiền DB Ở SQL Mới Gửi Bàn Lên AI Nơi Trần Tục (False = Đang Vẫn Lộn Cầu Stream Bị Cấn Lag Tạch Mất Rụng Đừng Có In Ra Cho Khách).
    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }
    // Lúc Gập Sổ Bói Chấm Dứt Xong Phim Coi.
    public DateTime? CompletedAt { get; private set; }

    // Dành cho Chiêu Gọi Của Máy Đọc Database Lưng Sau EF Core Cắn Dữ Liệu SQL Nên Gốc Cần Bắt Để Ẩn.
    public string? AiSummary { get; private set; }
    
    // Lưu các câu Followups dưới dạng (Question, Answer, Cost) thành list record hoặc class nội bộ.
    // Dùng List<ReadingFollowup> cho sạch
    public IReadOnlyList<ReadingFollowup> Followups { get; private set; } = new List<ReadingFollowup>();

    protected ReadingSession() { }

    /// <summary>
    /// Thuật Dọn Bàn Giao Cảo Mở Quẻ Mớ Căng Bắt Đầu: Chui Vào Là Chỉ Tạo Đóng Id Chưa Rút Lá Bơm Bọc Khách (Trừ Tiền Chưa Xong).
    /// </summary>
    public ReadingSession(string userId, string spreadType, string? question = null, string? currencyUsed = null, long amountCharged = 0)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        SpreadType = spreadType;
        Question = question;
        CurrencyUsed = currencyUsed;
        AmountCharged = amountCharged;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Cầm Chổi Lên Khẩy Lá Xong Hô Hồi Quẻ (Lấy DB Lên).</summary>
    public static ReadingSession Rehydrate(
        string id,
        string userId,
        string spreadType,
        string? question,
        string? cardsDrawn,
        string? currencyUsed,
        long amountCharged,
        bool isCompleted,
        DateTime createdAt,
        DateTime? completedAt,
        string? aiSummary = null,
        IReadOnlyList<ReadingFollowup>? followups = null)
    {
        return new ReadingSession(userId, spreadType, question, currencyUsed, amountCharged)
        {
            Id = id,
            CardsDrawn = cardsDrawn,
            IsCompleted = isCompleted,
            CreatedAt = createdAt,
            CompletedAt = completedAt,
            AiSummary = aiSummary,
            Followups = followups ?? new List<ReadingFollowup>()
        };
    }

    /// <summary>Chốt Sổ Cuộn Tranh Lá Ấn Triện AI Trả Về Khách.</summary>
    public void CompleteSession(string cardsDrawnJson)
    {
        CardsDrawn = cardsDrawnJson;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}

public class ReadingFollowup
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
