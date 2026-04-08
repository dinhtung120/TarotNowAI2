using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Phần hành vi tài khoản của User: kinh nghiệm, mật khẩu, hồ sơ và quyền/trạng thái vận hành.
public partial class User
{
    /// <summary>
    /// Cộng kinh nghiệm cho người dùng và tự động tăng cấp theo ngưỡng mỗi 100 EXP.
    /// Luồng xử lý: bỏ qua amount không hợp lệ, cộng EXP, tính level mới rồi cập nhật mốc UpdatedAt.
    /// </summary>
    public void AddExp(long amount)
    {
        if (amount <= 0)
        {
            // Edge case: không cộng EXP âm hoặc bằng 0 để tránh sai lệch tiến độ.
            return;
        }

        Exp += amount;
        var newLevel = 1 + (int)(Exp / 100);
        if (newLevel > Level)
        {
            Level = newLevel;
            // Chỉ tăng level khi EXP vượt mốc mới, không hạ cấp trong mọi trường hợp.
        }

        UpdatedAt = DateTime.UtcNow;
        // Ghi nhận thời điểm cập nhật hồ sơ sau thay đổi EXP/level.
    }

    /// <summary>
    /// Cập nhật mật khẩu đã hash cho tài khoản.
    /// Luồng xử lý: gán hash mới và cập nhật timestamp thay đổi.
    /// </summary>
    public void UpdatePassword(string newHash)
    {
        PasswordHash = newHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật thông tin hồ sơ hiển thị của người dùng.
    /// Luồng xử lý: gán displayName/avatar/dateOfBirth mới rồi cập nhật mốc thời gian.
    /// </summary>
    public void UpdateProfile(string displayName, string? avatarUrl, DateTime dateOfBirth)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Kích hoạt tài khoản để cho phép sử dụng đầy đủ chức năng hệ thống.
    /// Luồng xử lý: đặt Status về Active và cập nhật timestamp.
    /// </summary>
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Khóa tài khoản để chặn thao tác đăng nhập/sử dụng khi cần xử lý vi phạm.
    /// Luồng xử lý: đặt Status về Locked và cập nhật timestamp.
    /// </summary>
    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mở khóa tài khoản đã bị khóa trước đó.
    /// Luồng xử lý: đưa Status về Active và cập nhật timestamp.
    /// </summary>
    public void Unlock()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Nâng quyền tài khoản lên Admin khi được cấp phép quản trị.
    /// Luồng xử lý: gán Role = Admin và cập nhật mốc thời gian.
    /// </summary>
    public void PromoteToAdmin()
    {
        Role = UserRole.Admin;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Duyệt người dùng thành Reader và đồng bộ trạng thái xét duyệt.
    /// Luồng xử lý: chuyển role sang TarotReader, set ReaderStatus = Approved và cập nhật timestamp.
    /// </summary>
    public void ApproveAsReader()
    {
        Role = UserRole.TarotReader;
        ReaderStatus = ReaderApprovalStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Từ chối yêu cầu trở thành Reader của người dùng.
    /// Luồng xử lý: cập nhật ReaderStatus = Rejected và ghi nhận thời điểm thay đổi.
    /// </summary>
    public void RejectReaderRequest()
    {
        ReaderStatus = ReaderApprovalStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Khôi phục role và trạng thái reader từ snapshot trước đó.
    /// Luồng xử lý: gán lại role/readerStatus truyền vào và cập nhật timestamp.
    /// </summary>
    public void RestoreRoleAndReaderStatus(string role, string readerStatus)
    {
        Role = role;
        ReaderStatus = readerStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật role trực tiếp cho tài khoản theo quyết định quản trị.
    /// Luồng xử lý: gán role mới và cập nhật mốc UpdatedAt.
    /// </summary>
    public void UpdateRole(string newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }
}
