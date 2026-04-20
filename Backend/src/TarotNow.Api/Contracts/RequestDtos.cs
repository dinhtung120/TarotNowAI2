namespace TarotNow.Api.Contracts;

// Payload cập nhật hồ sơ cá nhân.
public class UpdateProfileRequest
{
    // Tên hiển thị mới của người dùng.
    public string DisplayName { get; set; } = string.Empty;

    // DEPRECATED: Avatar được cập nhật qua /api/v1/profile/avatar/confirm; field này giữ lại chỉ để tương thích ngược.
    public string? AvatarUrl { get; set; }

    // Ngày sinh phục vụ xác minh độ tuổi và cá nhân hóa.
    public DateTime DateOfBirth { get; set; }
}

// Payload tạo đơn nạp tiền.
public class CreateDepositOrderRequest
{
    // Mã gói nạp preset.
    public string PackageCode { get; set; } = string.Empty;

    // Khóa idempotency để chống tạo trùng đơn nạp khi client retry.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// Payload ghi nhận người dùng đã đồng ý tài liệu pháp lý.
public class RecordConsentRequest
{
    // Loại tài liệu đã đồng ý (ví dụ: Terms, Privacy).
    public string DocumentType { get; set; } = string.Empty;

    // Phiên bản tài liệu mà người dùng đã xác nhận.
    public string Version { get; set; } = string.Empty;
}

// Payload cập nhật thông tin khuyến mãi.
public class UpdatePromotionRequest
{
    // Ngưỡng nạp tối thiểu để kích hoạt khuyến mãi.
    public long MinAmountVnd { get; set; }

    // Số Gold thưởng khi đủ điều kiện.
    public long BonusGold { get; set; }

    // Trạng thái bật/tắt khuyến mãi.
    public bool IsActive { get; set; }
}
