/*
 * ===================================================================
 * FILE: UserWallet.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Domain Value Object Cực Kì Lớn Chứa Ví Riêng Nằm Gọn Lõn Bên Trong Thể Xác User (Owned Entity EF Core).
 *   Sử Dụng Khái Niệm SRP Tách Logic Túi Kim Cương Rõ Ràng Để Cầm Dắt Khỏi Bị Nặng DB Model User Và Bảo Mật Giữ Khảm Hạm.
 * ===================================================================
 */

using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Value Object (Hoặc Gọi Owned Entity) quản lý tài chính bọc sắt của người dùng.
/// 
/// Đã được Giải Phóng Tách Riêng khỏi User Lớn Để Giải Mỏi:
/// - Mảnh Ghép SRP (Single Responsibility Principle): Thằng User lớn ôm cả Đăng Nhập, Ảnh Ọc, Quyền Role Rồi Cho Nên Sẽ Bệnh To Béo. Nên Tách Nguyên Nhóm Kiếm Chác Trừ Tiền (credit, debit, freeze, refund) Vào Cái Tráp Riêng Chữ Tiền Này Sạch Nhất Quả Đất.
/// - Ngon Ăn Lúc Unit Test: Ôm Nhau Dễ Test Đoán Trừ Tiền Mà Bỏ Cứu Pass Mệt Đầu Ra Của User Entity Bự.
/// - Mở Tương Lai Dễ Thở: Rớt Cục Thạch Kim Cương Mai Thêm Tiền Khác Hay Vé Ép Thẻ Gọi (Shard) Thì Đóng Khối Code Ở Đây Chưa Tràn DB Sang.
///
/// Hoạt Động Đi Gắn Kiểu Gì Ở EF Core SQL?
/// - Chạy Owned Entity pattern: `builder.OwnsOne(u => u.Wallet, ...)` Cấu Hình Khóa Tốt Bóp Nát DB Trải Nằm 4 Cột Này Trên Cùng 1 Bảng "users" Nhưng Ở C# Code Lại Hiện Nguyên Class Ảo Túi Lập Trình. Sực Mùi Clean Architecture Sang Chảnh Của Giang Hồ Thế Giới OOP.
/// </summary>
public class UserWallet
{
    /// <summary>
    /// Đồng Vàng Lá Nghèo (Gold): tiền cày miễn phí — xem qc nhặt, lấy exp level lên nhận (Đéo Nạp Được Bằng VNPay Máu Thật). Khách Thích Chơi Xài Đồng Này Bói Rẻ Mức Kinh Ngạc Thường Rất Nhanh Hết Do Tham Lam Hỏi Nhiều 1 Phiên Mất 5 G Trừ Sạch.
    /// </summary>
    public long GoldBalance { get; private set; } = 0;

    /// <summary>
    /// Kim Cương Cứng Đỏ Tươi Lệ (Diamond): Tiền Quý Tộc Rút Tiền Húp Nạp Tươi Của Bọn Thanh Toán Bank Thẻ.
    /// Chi Phí VIP AI Đọc Đắt (Thầy Đọc Có Hoa Hồng Tầm Này Trừ Cực Đau Khách Xài Kim Cương Lãi Nhất App).
    /// </summary>
    public long DiamondBalance { get; private set; } = 0;

    /// <summary>
    /// Kim Cương Đang Bị Chặn Ở Khúc Giữa Oằn Nhau Escrow Không Thể Cầm Đi Chợ Tiêu Chỗ Khác (Frozen).
    /// Lúc Bám AI Tức Khắc Nhát Bắn: Trừ Từ Túi Trống Phải Chuyển Liền Lên Túi Treo Bóng Lưỡng Này (Qũy Chung Bóng Đèn -> Chờ AI Text Nhả Về Vừa Xem Đúng Nghĩa Thành Công Thật -> Tiêu Rụi Hoàn Toàn Gói Băng (Consume). AI Chết Đuổi Bắt Hụt Code Mạng Hỏng -> Rơi Lại Túi Trống Về Hoàn Nguyên Gốc Diamond Thường).
    /// </summary>
    public long FrozenDiamondBalance { get; private set; } = 0;

    /// <summary>
    /// Cột Đếm Góp Không Bao Giờ Tụt: Lưu Dấu Vết Tổng Cộng Mệnh Chủ Này Đã Đốt Nạp Bao Nhiêu Lớp Kim Cương (Không Dính Khuyến Mãi) Từ Đầu Tới Nay.
    /// Giúp Bọn Admin Cắt Cờ Thu Hụi Để Treo Gắn Tượng (VIP TIER Analytics Nữ Vương Tài Phiệt Bảng Xếp Hạng Khách Sộp).
    /// </summary>
    public long TotalDiamondsPurchased { get; private set; } = 0;

    /// <summary>
    /// Buộc Dành Cho Thằng EF Core SQL Khát Param Điếm Lúc Load Bóp Gọi Khống Chạy Từ Constructor SQL Lên Lấy Bọc.
    /// </summary>
    protected UserWallet() { }

    /// <summary>
    /// Ống Nặn Tạo Cái Túi Mủ Rỗng Túi Lúc New Acc Gắn Đón Thèn User Mới Khóc Ban Đầu Khởi Nghiệp Lên.
    /// </summary>
    public static UserWallet CreateDefault() => new UserWallet();

    // ======================================================================
    // MẠCH MÁU LUÂN CHUYỂN LOGIC RÚT THÊM TIỀN (METHODS CỘT LÕI)
    // ======================================================================

    /// <summary>
    /// Ném Tiền Cho Ví (Bơm Thêm Plus Tiền Tươi).
    /// Nếu Khách Deposit Rải Băng Tiền Gốc Có Dấu Vết Bank Dán Cạch Mới Tính Chút Exp Lên Cột Nạp Tổng.
    /// Bọc Lỗ Hổng Này Phải Sạch, Chống 0Đ Lót Tay Xuyên Cổng Lệnh Thức Đơn Để Đếm Chặn.
    /// </summary>
    public void Credit(string currency, long amount, string type)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền cộng vào phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            GoldBalance += amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            DiamondBalance += amount;
            // Dấu Gác Bắt Tổng Nạp Cho Rank Vip Nghe Điêm Đợi Lệnh Deposit Bank Vô Nghia.
            if (type == TransactionType.Deposit)
            {
                TotalDiamondsPurchased += amount;
            }
        }
        else
        {
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

    /// <summary>
    /// Khứa Đứt Móc Túi Lụm Đem Thui Ví Khách Mua Hàng (Debit Thụt Tiền Ác Ý Không Thương).
    /// Thủng Gò Hoặc Thuếu Tiền (Tiền Âm Lập Tức Chụp Lôi Về Hàm Domain Báo Nút Lầm Cầm InvalidOperationException Dẹp Cho Bọn Handler Application Cuộn Gói Ném Http Báo Về 400 Bad Request Cấm Lạm Phát DB).
    /// </summary>
    public void Debit(string currency, long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền trừ đi phải lớn hơn 0.", nameof(amount));

        if (currency == CurrencyType.Gold)
        {
            if (GoldBalance < amount)
                throw new InvalidOperationException("Số dư Gold không đủ.");
            GoldBalance -= amount;
        }
        else if (currency == CurrencyType.Diamond)
        {
            if (DiamondBalance < amount)
                throw new InvalidOperationException("Số dư Diamond không đủ.");
            DiamondBalance -= amount;
        }
        else
        {
            throw new ArgumentException($"Loại tiền tệ không hợp lệ: {currency}", nameof(currency));
        }
    }

    /// <summary>
    /// Nắm Cổ Túm Tiền Vứt Lên Cây Sào Giữ Chặt Đóng Băng Khối Trụ Nó Lại (Trừ Lòng Ví Giữ Thắng Kim Cương Chờ Oằn Phiên Trải Bài AI Chờ Đợi Phím Cuối).
    /// </summary>
    public void FreezeDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền đóng băng phải lớn hơn 0.", nameof(amount));

        if (DiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond không đủ để đóng băng.");

        DiamondBalance -= amount;
        FrozenDiamondBalance += amount;
    }

    /// <summary>
    /// Thước Kẻ Tuốt Áp Giải Cởi Cột Tảng Băng Nước (Mở Bỏ Sợi Khóa Bóng Bàng Cho Cục Diamond Chuyển Dịch Quyền Tiền Tệ Về Cửa Ngoài Đi Tới Thằng Chủ Nào Đó Ví Dụ Thầy Reader Đã Giao Code).
    /// Lệnh Này Chỉ Tịt Số Dư Giam Mất Đi (Không Cộng Đâu Cho Lòng Này Của Mất Đi - Mất Hút Do Rút Release Từ Mở).
    /// </summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền giải phóng phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để giải phóng.");

        FrozenDiamondBalance -= amount;
    }

    /// <summary>
    /// Nôn Oái Trả Hoàn Khách Gốc Vì AI Nguy Kịch / Hay Reader Treo Rớt Không Coi Rep Nhắn Cáu Nữa Ném Lại Nét Tốt Cho Ví Thằng Mua Lại (Refund Lấy Nước Trả Bể Đổ).
    /// </summary>
    public void RefundFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền hoàn trả phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để hoàn trả.");

        FrozenDiamondBalance -= amount;
        DiamondBalance += amount;
    }

    /// <summary>
    /// Bom Nuốt Mắc Phích Nổ Tung Hệ Đốt Sạch Bay Màu Thạch Khối Kim Cương (Bóc Phạt Cục Băng Giam Do Nhâm Cốt Bốc Bóp Gọi Mua AI Trực Tiếp Server Nát Mức Thành Tro Cho API Nuốt Tiền Không Có Release Trả Ai Cả, Cất Trữ Đi Đốt Lạc Đi Trôi Hầm Giới Phát Sinh Thành Chi Phí Phục Vụ Hãng AI OpenAI Chứ Ko Qua Reader Của Game).
    /// </summary>
    public void ConsumeFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền tiêu thụ phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để tiêu thụ.");

        FrozenDiamondBalance -= amount;
    }
}
