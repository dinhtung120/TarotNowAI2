using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Entity người dùng trung tâm của hệ thống, chứa thông tin tài khoản, trạng thái và hồ sơ ví.
public partial class User
{
    // Định danh người dùng.
    public Guid Id { get; private set; }

    // Email đăng nhập duy nhất.
    public string Email { get; private set; } = string.Empty;

    // Username hiển thị/đăng nhập duy nhất.
    public string Username { get; private set; } = string.Empty;

    // Mật khẩu đã hash.
    public string PasswordHash { get; private set; } = string.Empty;

    // Tên hiển thị của người dùng.
    public string DisplayName { get; private set; } = string.Empty;

    // Ảnh đại diện.
    public string? AvatarUrl { get; private set; }

    // Key object trên R2 (hoặc đường dẫn tương đương khi dùng local storage) để xóa khi đổi avatar.
    public string? AvatarObjectKey { get; private set; }

    // Ngày sinh.
    public DateTime DateOfBirth { get; private set; }

    // Tên ngân hàng rút tiền đã cấu hình (chỉ áp dụng cho Reader).
    public string? PayoutBankName { get; private set; }

    // Mã BIN ngân hàng theo chuẩn NAPAS/VietQR.
    public string? PayoutBankBin { get; private set; }

    // Số tài khoản nhận tiền.
    public string? PayoutBankAccountNumber { get; private set; }

    // Tên chủ tài khoản nhận tiền (chữ hoa không dấu).
    public string? PayoutBankAccountHolder { get; private set; }

    // Cờ người dùng đã chấp thuận điều khoản bắt buộc.
    public bool HasConsented { get; private set; }

    // Cấp độ hiện tại.
    public int Level { get; private set; } = 1;

    // Tổng EXP tích lũy.
    public long Exp { get; private set; }

    // Ví người dùng.
    public UserWallet Wallet { get; private set; }

    // Trạng thái tài khoản.
    public string Status { get; private set; } = string.Empty;

    // Vai trò người dùng (User/Admin/Reader...).
    public string Role { get; private set; } = string.Empty;

    // Trạng thái duyệt reader.
    public string ReaderStatus { get; private set; } = string.Empty;

    // Thời điểm tạo tài khoản.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; set; }

    // Cờ bật MFA.
    public bool MfaEnabled { get; set; }

    // Secret MFA đã mã hóa.
    public string? MfaSecretEncrypted { get; set; }

    // Danh sách mã backup MFA đã hash (JSON).
    public string? MfaBackupCodesHashJson { get; set; }

    // Streak hiện tại.
    public int CurrentStreak { get; private set; } = 0;

    // Ngày streak gần nhất.
    public DateOnly? LastStreakDate { get; private set; }

    // Streak trước khi bị đứt để phục hồi.
    public int PreBreakStreak { get; private set; } = 0;

    // Danh hiệu đang kích hoạt.
    public string? ActiveTitleRef { get; private set; }

    // Danh sách consent pháp lý của người dùng.
    public ICollection<UserConsent> Consents { get; private set; } = new List<UserConsent>();

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: khởi tạo wallet mặc định khi entity được dựng từ persistence.
    /// </summary>
    protected User()
    {
        Wallet = UserWallet.CreateDefault();
    }

    /// <summary>
    /// Khởi tạo người dùng mới với trạng thái pending và vai trò mặc định.
    /// Luồng xử lý: sinh id, gán thông tin đăng ký, tạo wallet mặc định và set trạng thái ban đầu.
    /// </summary>
    public User(
        string email,
        string username,
        string passwordHash,
        string displayName,
        DateTime dateOfBirth,
        bool hasConsented)
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

    /// <summary>
    /// Đặt danh hiệu đang hiển thị cho hồ sơ người dùng.
    /// Luồng xử lý: gán ActiveTitleRef và cập nhật mốc UpdatedAt.
    /// </summary>
    public void SetActiveTitle(string? titleRef)
    {
        ActiveTitleRef = titleRef;
        UpdatedAt = DateTime.UtcNow;
    }
}
