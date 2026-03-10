using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho người dùng hệ thống.
/// Mọi thao tác cập nhật (đổi pass, lên level) nên định nghĩa rõ thành method.
/// </summary>
public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string DisplayName { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    /// <summary>
    /// Thuộc tính này bắt buộc true khi đăng ký.
    /// Có thể versioning nếu Consent thay đổi trong OPS Phase 1.
    /// </summary>
    public bool HasConsented { get; private set; }

    // Level & Exp hiện tại
    public int Level { get; private set; } = 1;
    public long Exp { get; private set; } = 0;

    // Dành cho Wallet
    public long GoldBalance { get; private set; } = 0;
    public long DiamondBalance { get; private set; } = 0;
    public long FrozenDiamondBalance { get; private set; } = 0;
    public long TotalDiamondsPurchased { get; private set; } = 0;

    public string Status { get; private set; }
    public string Role { get; private set; }
    public string ReaderStatus { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Dành cho EF Core
    protected User() { }

    public User(string email, string username, string passwordHash, string displayName, DateTime dateOfBirth, bool hasConsented)
    {
        Id = Guid.NewGuid();
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
        HasConsented = hasConsented;
        Status = UserStatus.Pending; // Cần verify email mới đổi sang Active
        Role = UserRole.User;
        ReaderStatus = ReaderApprovalStatus.Pending;
        CreatedAt = DateTime.UtcNow;
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
    /// Kích hoạt tài khoản khi OTP hợp lệ.
    /// </summary>
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cộng tiền vào ví (Gold hoặc Diamond). Nếu là nạp thẻ (Deposit), tính thêm vào số tổng Diamond đã mua.
    /// Giải thích: Logic trước đây nằm ở Postgres proc_wallet_credit, nay chuyển về Entity để dễ quản lý.
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
            if (type == TransactionType.Deposit)
            {
                TotalDiamondsPurchased += amount;
            }
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Trừ tiền từ ví. Sẽ throw exception nếu số dư không đủ.
    /// Giải thích: Đảm bảo biến động số dư luôn hợp lệ trước khi persist DB.
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

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đóng băng (Freeze) Diamond - chuyển từ khả dụng sang đang bị giữ (Escrow).
    /// </summary>
    public void FreezeDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền đóng băng phải lớn hơn 0.", nameof(amount));

        if (DiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond không đủ để đóng băng.");

        DiamondBalance -= amount;
        FrozenDiamondBalance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Giải phóng (Release) Diamond - trừ Diamond đã đóng băng của người trả tiền (Payer).
    /// Không cộng vào đây, vì Diamond sẽ được cộng (Credit) sang người nhận ở transaction khác.
    /// </summary>
    public void ReleaseFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền giải phóng phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để giải phóng.");

        FrozenDiamondBalance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Hoàn trả (Refund) Diamond - chuyển từ đóng băng về lại khả dụng.
    /// </summary>
    public void RefundFrozenDiamond(long amount)
    {
        if (amount <= 0) 
            throw new ArgumentException("Số tiền hoàn trả phải lớn hơn 0.", nameof(amount));

        if (FrozenDiamondBalance < amount)
            throw new InvalidOperationException("Số dư Diamond đóng băng không đủ để hoàn trả.");

        FrozenDiamondBalance -= amount;
        DiamondBalance += amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
