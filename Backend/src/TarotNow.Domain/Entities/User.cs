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
    public string Email { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string DisplayName { get; private set; }
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

    public string Status { get; private set; }
    public string Role { get; private set; }
    public string ReaderStatus { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

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

