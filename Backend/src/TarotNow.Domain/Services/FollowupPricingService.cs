/*
 * ===================================================================
 * FILE: FollowupPricingService.cs
 * NAMESPACE: TarotNow.Domain.Services
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Service Quy Định Áp Giá Hỏi Khách (Hút Máu Bào Bao Nhiêu Kim Cương Trong Chat Dựa Vào Lá Thẻ Quý).
 *   Logic Tính Lãi Mặn Khét Lẹt Thuê Dựa Do AI Khét Giỏ (Phase 1.5).
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TarotNow.Domain.Services;

/// <summary>
/// Domain Service Phán Gía Cước Follow-up Chat Bóc Lột Client.
/// Thang Giá Leo Lề Đâm Dài (1 Hỏi Thêm Tiếp Câu Thưởng Miễn, Càng Cố Ánh Hỏi Càng Leo Thang x2 Móc Giá Kim Cương (Mũ Lũy Thừa 1 2 4 8 16 Diamond).
/// Game Phụ Miễn Cước Free Quota Tùy Phúc Phần May Mắn Lúc Rút Quẻ Khớp Lá Càng Hiếm -> Càng Nhiều Slot Hỏi Chùa Không Tính Đá (Phân Thưởng Bất Ngờ Thúc Game Lên Ngôi Cáo).
/// </summary>
public class FollowupPricingService
{
    // Bạc Ráp Rạch Đắng Tăng Phí Nẹp: Càng Mở Giá Ép Nhập Gài [1 Kim, Xong Ép 2 Kim, Giá Gấp 4 Kim, Phạt Gấp 8...].
    private static readonly int[] PriceTiers = new[] { 1, 2, 4, 8, 16 };
    // Đoạn Bịt Sừng Sỏ Chống Chat Rác Nuốt Limit (5 Câu Cù Nhai Maximum Chết Ca Nép Đập Box Cấm Gửi Hỏi Rẽ Buộc Trải Trận Bói Mới Sạch Sẽ App Lợi Bank Nạp Cuốc Khác Lấy Quota Trí AI).
    public const int MAX_FOLLOWUPS_ALLOWED = 5;

    // Giả Lập Biến Thể Rank Độ Hiếm Của Bài Lá Phẩm Giá Ở Tầng Database Khớp Cục Này Cứng Nhắc Lấy ID Bài Lọc Trình Level Mật Độ "Sang Trọng".
    public int GetMockCardLevel(int cardId)
    {
        if (cardId < 0 || cardId > 77) return 1; // Lá Bậy Lọc Đọc Nạp Mã Lỗi Khùng Cho Xếp Bét Hạng.

        // Minor Xoạc Vô Major Xịn Khang Trang (Bọn Bói Chính Chói Kính Trùm)
        if (cardId <= 21)
        {
            // Bài Cốt Vang Quyền The Fool Nảy Mâm -The World Có Thần Cách Nhảy Lên Level Quét 10 Móc Ra Điểm Tích Xa Trọng Lực Cộng Thêm Điểm Nhánh Rank Gốc Phái 10+21.
            return 10 + cardId;
        }
        else
        {
            // Số Lép Chót Phỏn (Minor Arcana Như Bọn Kiếm Gậy Gốc Rác Cho Bóc Đọc Lên Tệ). 
            return (cardId % 14) + 1; // Chừa Xoạc Các Hệ Modulo Trả Gục Về 1 Level Tới 10 Nút Rẻ Tè Cho Minor Card Đẩy Cùi Bắp Quyền Hút Lỗi (King Trí 14 Chém).
        }
    }

    /// <summary>
    /// Rọi Đèn Quét Xem Trong Lòng Json Bà Thằng Khách Vừa Bóc Session Mớ Bài Nào Có Con Boss Cỡ Sự Rực Lửa Không Để Phán Cấp Slot Bói Miễn Phí (Trừ Giá Zero Thức Đầu Tư Kim Cương).
    /// </summary>
    public int CalculateFreeSlotsAllowed(string cardsDrawnJson)
    {
        if (string.IsNullOrWhiteSpace(cardsDrawnJson)) return 0; // Trúc Bóp Trải Trống Ép Độc 0 Slot Mõm

        try
        {
            // Bẻ Mã JSON ID Trúc Database Móc Từ 25, 4 Mấy Nắm Bài ID Lôi Chạy Ngược.
            var cardIds = JsonSerializer.Deserialize<int[]>(cardsDrawnJson) ?? Array.Empty<int>();
            if (!cardIds.Any()) return 0;

            // Nhổ Cục Nấm Lớn Nhất Đọc Độ Hiếm Đắt (Max Thụ Phóng Nhất Cho Slot Không Xét Cả Làng Nhũn Tưởng). 
            int highestLevel = cardIds.Max(GetMockCardLevel);

            if (highestLevel >= 16) return 3; // Lệnh Chớp 16 Đáy Vú Nhận 3 Tràng Free.
            if (highestLevel >= 11) return 2; // Gốc Vua King Đc Tới Tận Cây Bóc 2 Tràng.
            if (highestLevel >= 6) return 1;  // Đủ 6 Ép Kiếm Ngọt Được Vớt 1 Nháy Follow-up.

            return 0; // Nghèo Chớp Cua Vít Rễ Lửa Thì Nạp Thạch Méo Đc Khuyến Miễn.
        }
        catch
        {
            return 0; // Parse JSON Lỗi Vục Cuốc Tát Đập Error Cũng Éo Nạp Nén Null Null Bét Trả Chết Lỗi Để Khỏi Cãi Ác Ý.
        }
    }

    /// <summary>
    /// Máy Nổ Kim Vang: Tính Phác Xem Hỏi 1 Phát Mõm Này Là Khứa Sẽ Mất Nhất Chém Mấy Phí Trả Giáng Cứng Nhanh Cho Application Layer.
    /// Trả 0 Thì Tức Qua Cửa (Có Quỷ Bài Free Đỡ Giùm), Trả Số Lớn Tức App Trừ Tiền Kí Phạt Diamond Xuất Hiện.
    /// </summary>
    public int CalculateNextFollowupCost(string cardsDrawnJson, int currentFollowupCount)
    {
        // 1. Phập Gate Đóng Trục Cửa Không Cứu (Nội Gián Đòi Hỏi Hơn 5 Nhát Khóa Đập App Báo Lỗi Bắt Tức Giằng Bắt Trải Session Khác - Chặn Spam Hủy AI Lỗ).
        if (currentFollowupCount >= MAX_FOLLOWUPS_ALLOWED)
        {
            throw new InvalidOperationException($"Đã đạt giới hạn tối đa {MAX_FOLLOWUPS_ALLOWED} câu hỏi phụ cho phiên rút bài này.");
        }

        // Húp Máy Soi Độ Cháy Ác Kiếm Số Câu Lỗ Phí Chui Từ Bảng Thần Phía Trái
        int freeSlots = CalculateFreeSlotsAllowed(cardsDrawnJson);

        // Bọn Vẫn Trong Lưới Trừ (Thâm Quyền Thưởng Hút Đớ Chưa Hết Túi Lỗ)
        if (currentFollowupCount < freeSlots)
        {
            return 0; // Phóng Đút Lệnh Free Pass 0 Không Tiêu Cho Quét Trú 0 Diamond Hoàn Khỏi Tròn Nhốt Nợ.
        }

        // Tàn Tro Ăn Má Phí (Thật Ép Đáy Xẹp Phạt Mạng Ở Index Trừ Sào Bắt Bấu Chút)
        int paidTierIndex = currentFollowupCount - freeSlots;

        // Tránh Cáo Kẹt Vạch Vội Cho Gà Cào Vượt Hết Lòng Dãy Giả Bảng Mảng Kê Mức Cuối Out Of Range Error Ở Tầng C#.
        if (paidTierIndex >= PriceTiers.Length)
        {
            paidTierIndex = PriceTiers.Length - 1; // Sáp Thét Má Đâm Cố Tới Giá Kịch Trần Cuối (16 Kim).
        }

        return PriceTiers[paidTierIndex]; // Vác Cho Gọi Dịch Đón Gắng Nuốt Phạt Ở Vó Dao Tiền Khóa Lũy Kế Nạp.
    }
}
