using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho người dùng hệ thống.
/// 
/// Refactored theo SRP (Single Responsibility Principle):
/// - User: quản lý thông tin cá nhân, trạng thái tài khoản, level/EXP.
/// - UserWallet (Owned Entity): quản lý tài chính (Gold, Diamond, Escrow).
/// 
/// Trước đây User chứa 232 dòng với 7 methods tài chính — vi phạm SRP.
/// Nay tách ra, User chỉ giữ ~120 dòng với trách nhiệm rõ ràng.
/// </summary>
public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string? AvatarUrl { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    /// <summary>
    /// Thuộc tính này bắt buộc true khi đăng ký.
    /// Có thể versioning nếu Consent thay đổi trong OPS Phase 1.
    /// </summary>
    public bool HasConsented { get; private set; }

    // Level & Exp hiện tại
    public int Level { get; private set; } = 1;
    public long Exp { get; private set; } = 0;

    /// <summary>
    /// Owned Entity quản lý toàn bộ tài chính.
    /// EF Core sẽ map các property của Wallet vào cùng bảng "users".
    /// Truy cập: user.Wallet.GoldBalance, user.Wallet.Credit(...), v.v.
    /// </summary>
    public UserWallet Wallet { get; private set; }

    // ======================================================================
    // BACKWARD COMPATIBILITY: Các property delegate sang Wallet
    // Giữ lại để không phải sửa tất cả query/projection cùng lúc.
    // Có thể loại bỏ dần trong các sprint tiếp theo.
    // ======================================================================
    public long GoldBalance => Wallet.GoldBalance;
    public long DiamondBalance => Wallet.DiamondBalance;
    public long FrozenDiamondBalance => Wallet.FrozenDiamondBalance;
    public long TotalDiamondsPurchased => Wallet.TotalDiamondsPurchased;

    public string Status { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public string ReaderStatus { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Phase 2.5 — MFA (TOTP)
    public bool MfaEnabled { get; set; }
    public string? MfaSecretEncrypted { get; set; }
    public string? MfaBackupCodesHashJson { get; set; }

    public ICollection<UserConsent> Consents { get; private set; } = new List<UserConsent>();

    // Dành cho EF Core
    protected User() 
    {
        Wallet = UserWallet.CreateDefault();
    }

    public User(string email, string username, string passwordHash, string displayName, DateTime dateOfBirth, bool hasConsented)
    {
        Id = Guid.NewGuid();
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
        HasConsented = hasConsented;
        Status = UserStatus.Pending;
        Role = UserRole.User;
        ReaderStatus = ReaderApprovalStatus.Pending;
        Wallet = UserWallet.CreateDefault();
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsConsented()
    {
        HasConsented = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cộng điểm kinh nghiệm (EXP) và xử lý thăng cấp (Level Up).
    /// Quy tắc: 100 EXP = 1 Level.
    /// </summary>
    public void AddExp(long amount)
    {
        if (amount <= 0) return;

        Exp += amount;
        
        // Logic thăng cấp: Mỗi 100 EXP lên 1 cấp
        // Ví dụ: 105 EXP -> Level 2 (nếu bắt đầu từ Level 1)
        int newLevel = 1 + (int)(Exp / 100);
        if (newLevel > Level)
        {
            Level = newLevel;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật mật khẩu sau khi hash thành công.
    /// </summary>
    public void UpdatePassword(string newHash)
    {
        PasswordHash = newHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật thông tin Profile của User.
    /// </summary>
    public void UpdateProfile(string displayName, string? avatarUrl, DateTime dateOfBirth)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Kích hoạt tài khoản khi OTP hợp lệ.
    /// </summary>
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Khóa tài khoản (Admin gọi).
    /// </summary>
    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mở khóa tài khoản (Admin gọi).
    /// </summary>
    public void Unlock()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Nâng cấp người dùng lên quyền Admin (Dùng cho Seed/Setup ban đầu).
    /// </summary>
    public void PromoteToAdmin()
    {
        Role = "admin";
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Phê duyệt user trở thành Tarot Reader (Admin gọi).
    ///
    /// Quy trình: User submit request → Admin approve → gọi method này.
    /// - Chuyển Role từ "user" sang "tarot_reader" để mở quyền Reader.
    /// - Chuyển ReaderStatus từ "pending" sang "approved" để đánh dấu đã duyệt.
    /// - Sau khi gọi method này, hệ thống cần tạo reader_profiles document
    ///   trong MongoDB (xử lý ở Application layer, không phải ở đây).
    /// </summary>
    public void ApproveAsReader()
    {
        Role = UserRole.TarotReader;
        ReaderStatus = ReaderApprovalStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Từ chối yêu cầu đăng ký Reader (Admin gọi).
    ///
    /// - Giữ nguyên Role = "user" (không thay đổi quyền).
    /// - Chuyển ReaderStatus sang "rejected" để user biết bị từ chối.
    /// - User có thể submit lại request mới sau khi bị reject.
    /// </summary>
    public void RejectReaderRequest()
    {
        ReaderStatus = ReaderApprovalStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    // ======================================================================
    // BACKWARD COMPATIBILITY: Delegate methods sang Wallet
    // Giữ lại để WalletRepository không bị breaking change.
    // Các handler/repo gọi user.Credit() → thực tế gọi user.Wallet.Credit()
    // Sẽ loại bỏ dần khi migrate xong toàn bộ callers sang user.Wallet.*
    // ======================================================================

    /// <summary>Delegate → Wallet.Credit</summary>
    public void Credit(string currency, long amount, string type)
    {
        Wallet.Credit(currency, amount, type);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Delegate → Wallet.Debit</summary>
    public void Debit(string currency, long amount)
    {
        Wallet.Debit(currency, amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Delegate → Wallet.FreezeDiamond</summary>
    public void FreezeDiamond(long amount)
    {
        Wallet.FreezeDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Delegate → Wallet.ReleaseFrozenDiamond</summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        Wallet.ReleaseFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Delegate → Wallet.RefundFrozenDiamond</summary>
    public void RefundFrozenDiamond(long amount)
    {
        Wallet.RefundFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Delegate → Wallet.ConsumeFrozenDiamond</summary>
    public void ConsumeFrozenDiamond(long amount)
    {
        Wallet.ConsumeFrozenDiamond(amount);
        UpdatedAt = DateTime.UtcNow;
    }
}
