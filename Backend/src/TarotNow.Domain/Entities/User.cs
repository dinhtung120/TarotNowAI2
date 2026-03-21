/*
 * ===================================================================
 * FILE: User.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cột Trụ Trung Tâm Gánh Khả Năng Lưu Trữ Thông Tin Khách: Domain Entity User Nhúng EF Core Tỏa Vào SQL Postgres.
 *   Là Bức Tâm Thư Định Tuyến Của Ánh Sáng Quản Lý Role (User Hay Thầy Reader Lộ Cầm Phím Màn Tráng), Kèm Cả Thùng Túi Đựng Kim Cương Owned Entity Trực Ánh Lên User (SRP).
 * ===================================================================
 */

using TarotNow.Domain.Enums;
using System;
using System.Collections.Generic;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho người dùng hệ thống Gọi Ra To To Gộp (Bảng users SQL).
/// 
/// Đã Nâng Cấp Tách Giải Cho Nguyên Tắc Trách Nhiệm Đơn (SRP - Single Responsibility Principle):
/// - User Ở Này: Chỉ Cắm Đầu Quản Bóp Quyền Lực Đăng Nhập Thông Tin Tuổi Tác, Level Mệnh Hỏa.
/// - UserWallet Tự Chạy Đi Nấp Giấu Trong 1 Object Con (Owned Entity Ở Trong Đây) Quản Lý Kim Cương Đồng Vàng Riêng Lẻ Đem Gom Vào Cho Giảm Tải Ôm Đồ Quá Lớn Tránh User Code Dài Miên Man Bẩn DB.
/// </summary>
public class User
{
    // Căn Giấy Phép Định Danh Người Chơi
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    
    // Nêm Chặt Mật Mã Nấu Canh Lọc Không Lưu Lời Cũ.
    public string PasswordHash { get; private set; } = string.Empty;
    
    public string DisplayName { get; private set; } = string.Empty;
    // Ảnh Web Lưu (Gắn S3 Hoặc Đọc CDN Quăng Vào Cloudinary Chữ URL Rớt Vào Này).
    public string? AvatarUrl { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    /// <summary>
    /// Vòi Quấn Đóng Chốt (Chỉ Người Dùng Bật Tích Chấp Nhận ToS Mới Bóp Xong Đăng Kí Này Nằm Ở True Suốt Kiếp Chơi Game).
    /// </summary>
    public bool HasConsented { get; private set; }

    // Kho Tích Góp Level Gacha. Cày Xem Nhiễu Cày Nạp Vàng EXP Tụt Vào.
    public int Level { get; private set; } = 1;
    public long Exp { get; private set; } = 0;

    /// <summary>
    /// Cục Vàng Tỏa Ánh Sáng (Owned Entity - Bóp DB EF Core Kê Đều Vành SQL Tụ Chung Dữ Liệu Ví Nằm Lì Cùng Bảng User Mặc Chỗ Lập Trình Tách Gọn Thư Mục Tách Thành Thể Mới Rớt Xuống Đọc Ở Thẻ Wallet Đóng Gói Nhỏ Tiền Cho Khỏi Quên Gì).
    /// Kéo User Lên Trả Theo "user.Wallet.GoldBalance" Tránh Để Tùm Lùm User.GoldBalance Xấu Mã Hướng Đối Tượng Không Sạch SRP Ngập Mặt.
    /// </summary>
    public UserWallet Wallet { get; private set; }

    // ======================================================================
    // BACKWARD COMPATIBILITY (Xưa Quê Viết Hẹp Nay Code Nhiều Chỗ Đi Kẹp Nên Vẫn Để Cũ Mượn Code Đầu Đặng Đừng Rớt Rớt Dăm Cái Unit Test To).
    // Phô Lòi Các Method Con Của Túi Tiền Lên Ngoài Cho Tiện Lợi Bọn File Infrastructure Gọi Trúng Giả Lấy Từ Ruột Wallet Cũ Trả Giả Cho Đâu Lộ (Đến Mùa Migrate Tách Khỏi Vùng Thì Bỏ Mấy Cái Link Cưới Bất Ly Thân Đi Lại).
    // ======================================================================
    public long GoldBalance => Wallet.GoldBalance;
    public long DiamondBalance => Wallet.DiamondBalance;
    public long FrozenDiamondBalance => Wallet.FrozenDiamondBalance;
    public long TotalDiamondsPurchased => Wallet.TotalDiamondsPurchased;

    // Trạng Thái Khóa Treo Ngang Mệnh (Lock Mõm, Active Ngon Lệ...).
    public string Status { get; private set; } = string.Empty;
    // Sứ Mệnh Chức Vụ Hệ Thống (admin hay user đen hay Cày Reader Thuê Thuyết Pháp 'tarot_reader').
    public string Role { get; private set; } = string.Empty;
    // Status Của Quá Trình Xin Vào Cuộc Lăng Xê Làm Thầy Của User (Được Chấp Hay Vọng Vứt Dưới Bãi Rác Bọc: pending, approved, rejected).
    public string ReaderStatus { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Phase 2.5 — Vọc MFA Code (App Security 6 Số Phone Authenticator Bảo Toàn Ngục Ví Lớn).
    public bool MfaEnabled { get; set; }
    public string? MfaSecretEncrypted { get; set; }
    public string? MfaBackupCodesHashJson { get; set; }

    // Kết Liên Collection Của Đóng Gói Consent Từng Bản Hợp Đồng 1 Của Ai (EF Core Include Lấy Một Loạt V2 V3 Nhét Liên Túi DB 1-N).
    public ICollection<UserConsent> Consents { get; private set; } = new List<UserConsent>();

    protected User() 
    {
        Wallet = UserWallet.CreateDefault();
    }

    /// <summary>Sinh Em Pé Ra Đời Với Default Cơm Làng Túi Trắng.</summary>
    public User(string email, string username, string passwordHash, string displayName, DateTime dateOfBirth, bool hasConsented)
    {
        Id = Guid.NewGuid();
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
        HasConsented = hasConsented;
        Status = UserStatus.Pending; // Chờ Verify 4 Cẳng
        Role = UserRole.User;
        ReaderStatus = ReaderApprovalStatus.Pending;
        Wallet = UserWallet.CreateDefault();
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đeo Vòng Buff Lực Cày Cho Thằng User Lên Giới Lấy Exp. Vượt 100 Nhát Exp Thì Xé Tầng Thở Nâng Thể Level 2 Ánh Kim Lên Kéo Tội Thả Tim Nhóm Code Trăm Mép Cắt.
    /// </summary>
    public void AddExp(long amount)
    {
        if (amount <= 0) return;

        Exp += amount;
        
        // Nhào Thưởng Code Lên Level 
        int newLevel = 1 + (int)(Exp / 100);
        if (newLevel > Level)
        {
            Level = newLevel;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Thay Đổi Cấu Trúc Khóa Vàng (Mật Khẩu Mới Ném Mã Phức Tạp Vô Đè Dấu).</summary>
    public void UpdatePassword(string newHash)
    {
        PasswordHash = newHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Upload Thay Bức Mặt Kẻ Dịch Cử Tên.</summary>
    public void UpdateProfile(string displayName, string? avatarUrl, DateTime dateOfBirth)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Phép Nhấn Nốt Qua Sông Sau Khi Hòm Mail OTP Đã Khớp Verify Nhé Dâu Thơm Mùi Quýt Sống Thoải Mái App.</summary>
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Nút Cho Sếp Túm Rác Ném Cổng Cấm Login Vi Phạm Do Lá Báo Cáo Kiện Bẩn Report Quá (Khóa Account).</summary>
    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Đôn Quyền Hoàng Gia Admin Gắn Khí Bá (Seed Role Hoặc Cấp Cực Sếp Mới Nhận Hàng).</summary>
    public void PromoteToAdmin()
    {
        Role = "admin";
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Xả Hơi Phép Cho Tụi Khách Nó Đã Được Thủy Phổ Đầu Trở Thành Ông Quan Thầy Bói Sau Quá Trình Thi Đậu Bát Quái Cực Bự Mới Vãi (Reader Approved).
    /// Gắn Chức Nhóm Mới 'tarot_reader' Nên Thằng Thầy Mới Thấy Chữ Trang Phân Quyền Hàng Chờ Lựa Tiền Rút Mở Tùy Cho Tự Kẻ Menu "My Profile Reader" Kéo Mongo.
    /// </summary>
    public void ApproveAsReader()
    {
        Role = UserRole.TarotReader;
        ReaderStatus = ReaderApprovalStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Admin Bỏ Mõm Mất Uy Tín Đánh Rớt Đơn Xin Việc Về Mo. Thằng Này Có Nộp Lập List Khảo Còn Hiện DB Chờ Giải Kính Tòa Đạp Khóa Làm Lại Tự Đầu Mép.</summary>
    public void RejectReaderRequest()
    {
        ReaderStatus = ReaderApprovalStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Cháy Hàng Cảnh Lưới Compensation Lệnh Unit Bể Ống Xóa Undo Trả Nhả Về Trạng Thái Chưa Hề Xét Reader Hay Xin Được Vị Lòng Database SQL Cho Gọn Đẹp Chống Rách Rollback Dữ Liệu Thiếu Mongo Không Tao Lệnh Hoàn DB Nhấn.</summary>
    public void RestoreRoleAndReaderStatus(string role, string readerStatus)
    {
        Role = role;
        ReaderStatus = readerStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Thay đổi chức vụ (Role) của người dùng từ Admin UI.</summary>
    public void UpdateRole(string newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

    // ======================================================================
    // THỪA KẾ CHO NHANH: Ném Qủa Múc Ví Truyền Khớp Proxy Về Tận Entity Con Bọc Nó Chặn Rườm Rà Cho API Repo Đừng Đứt Refactor Giữ Sạch Cũ Bê Tới Thể Khơi Nguồn.
    // ======================================================================
    public void Credit(string currency, long amount, string type)
    {
        Wallet.Credit(currency, amount, type);
        UpdatedAt = DateTime.UtcNow;
    }
    public void Debit(string currency, long amount)
    {
        Wallet.Debit(currency, amount);
        UpdatedAt = DateTime.UtcNow;
    }
    public void FreezeDiamond(long amount)
    {
        Wallet.FreezeDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
    public void ReleaseFrozenDiamond(long amount)
    {
        Wallet.ReleaseFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
    public void RefundFrozenDiamond(long amount)
    {
        Wallet.RefundFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
    public void ConsumeFrozenDiamond(long amount)
    {
        Wallet.ConsumeFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
}
