/*
 * ===================================================================
 * FILE: UserCollection.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Khung Tướng Đích Thực Gắn Vào Bảng SQL `user_collections`: Là Rương Đựng Các Tờ Thẻ Bài Gốc Tarot Đã Rút Được Của Từng Quái Đứa (Tính Chức Sưu Tập - Collection).
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho Một Ngăn Rương Cất Thẻ Tarot (Vd: Khách Mới Rút Được Quân Mã Tarot Magician Có Bảng ID Sưu Tập Sáng Ép Đủ Điểm Chưa Thăng).
/// </summary>
public class UserCollection
{
    // Căn Xác Thức Đối Chủ Bộ Bài Này Bứt (Mã Chủ).
    public Guid UserId { get; private set; }
    // Khí Chỉ Định Đúng Mã Tên Quẻ Thẻ Card Mất Xếp Bị Nằm Đồ Lại Trong Thùng SQL Không Lẫn Vào Ai (0-77 Index 78 Tarot Card Đầu).
    public int CardId { get; private set; } 
    
    // Đã Up Level Thẻ Chưa Nhờ Ghép Chồng Thẻ Phôi Ép Thẻ Xịn Khủng Vào Bóp Thẻ Gốc Gacha? (Cấp Tướng).
    public int Level { get; private set; }
    // Nắm Trong Tay Mấy Con Chó Bài Duplicate Nhau Gặp Khúc Nối Môn Vận Xui Rút Toàn Thằng Chữ (Càng Bốc Trùng Copies Đè Số Chập Lên Quái).
    public int Copies { get; private set; }
    // Kiếp Sức Cày Thưởng Điểm Exp Cho Nhát Card Đặc Bí Vẻn Tụ Đong Báo Danh Rút Lại Bức Có Trùng Chống Chéo Nâng Bật Cả Lá 1 To Lên Exp Của Bổ Tướng Card Này Exp Gained Riêng Cho Bài Sẽ Xay Ra Level Lên Tiên.
    public long ExpGained { get; private set; }
    // Ngày Gần Khít Níu Vào Nhát Quay Thẻ Được Card Này Mới Hay Tũ Giảng Nát Ở Cổ Vùng (Quay Vào Năm Ngoái).
    public DateTime LastDrawnAt { get; private set; }

    protected UserCollection() { } // Dành Cho Database Vọc EF Core Tách

    /// <summary>Cầm Lá Được Ép Cứng Ở Máy Gacha Đầu Khi Chưa Từng Mở Rương.</summary>
    public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        ExpGained = 0;
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>Úm Ba La Kép Database Trực Trào Phọt Nguyên Con User Colection Trạng Nhĩ Kéo Form Nổi Phục Cả Level Xịn Lên Gấp Dịch Vụ API.</summary>
    public static UserCollection Rehydrate(UserCollectionSnapshot snapshot)
    {
        return new UserCollection(snapshot.UserId, snapshot.CardId)
        {
            Level = snapshot.Level,
            Copies = snapshot.Copies,
            ExpGained = snapshot.ExpGained,
            LastDrawnAt = snapshot.LastDrawnAt
        };
    }
    
    /// <summary>
    /// Vòi Sửa Đạo Thưởng Ánh Sáng Thẻ Lập Kép Ép Copy Thằng Trùng Mặt Tướng Này Thêm Cục Điểm Cao Cho Nhanh.
    /// Có Bơm EXP Thì Báo Sáng Cho Khác Thẻ Nếu Mà Mức Copies Được Bội Thu Đu Tới Mọi Bước (Hiện Tại Rules Đơn Giản Cho Cắt Thẻ Ra Nộp 5 Phôi Giống Khít Sẽ Thêm Chấm Vàng 1 Level).
    /// </summary>
    public void AddCopy(long expToGain)
    {
        Copies += 1;
        ExpGained += expToGain;
        LastDrawnAt = DateTime.UtcNow;

        // Luật Cổ Gacha Ép Xổ Cổ Tiên Hầu Vọc Máy Nhanh: 5 Phôi Trùng Bằng 1 Ngấn Cột Tướng Card Ép Level Lên Tranh Ảnh (Giả Chỉnh Bỏ Đây Tỏa Ra Nếu Thêm Config Khác Mặc Mong Dẫn Này Cứng Chắn Phê Trạm Hơi Cục Tạm Trong File Class Giữ Rules Rắn Domain Tương Cục).
        if (Copies % 5 == 0)
        {
            Level += 1;
        }
    }
}

public sealed class UserCollectionSnapshot
{
    public Guid UserId { get; init; }
    public int CardId { get; init; }
    public int Level { get; init; }
    public int Copies { get; init; }
    public long ExpGained { get; init; }
    public DateTime LastDrawnAt { get; init; }
}
