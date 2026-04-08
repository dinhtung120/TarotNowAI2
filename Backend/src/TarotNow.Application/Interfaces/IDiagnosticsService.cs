namespace TarotNow.Application.Interfaces;

// Contract tác vụ chẩn đoán hệ thống để hỗ trợ kiểm tra dữ liệu nền và thống kê vận hành.
public interface IDiagnosticsService
{
    /// <summary>
    /// Seed tài khoản admin mặc định trong môi trường cần bootstrap nhanh.
    /// Luồng xử lý: kiểm tra cấu hình seed, tạo hoặc bỏ qua tài khoản và trả kết quả trạng thái.
    /// </summary>
    Task<SeedAdminResult> SeedAdminAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Seed dữ liệu gamification mẫu để phục vụ kiểm thử hoặc chạy thử môi trường.
    /// Luồng xử lý: nạp dữ liệu nền cần thiết cho module gamification theo bộ chuẩn.
    /// </summary>
    Task SeedGamificationDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thống kê chẩn đoán để đánh giá tình trạng dữ liệu hiện tại của hệ thống.
    /// Luồng xử lý: tổng hợp số liệu cần thiết và trả DTO thống kê cho tầng gọi.
    /// </summary>
    Task<DiagnosticsStatsResult> GetStatsAsync(CancellationToken cancellationToken = default);
}

// Trạng thái kết quả seed tài khoản admin.
public enum SeedAdminStatus
{
    // Seed thành công hoặc đã đạt trạng thái mong muốn.
    Success = 1,

    // Cấu hình seed thiếu hoặc sai nên không thể thực thi.
    InvalidConfiguration = 2
}

// Kết quả chi tiết của thao tác seed admin.
public sealed class SeedAdminResult
{
    // Trạng thái seed cuối cùng.
    public SeedAdminStatus Status { get; init; }

    // Thông điệp mô tả kết quả để ghi log/vận hành.
    public string Message { get; init; } = string.Empty;

    // Email admin liên quan đến thao tác seed.
    public string? Email { get; init; }

    // Username admin liên quan đến thao tác seed.
    public string? Username { get; init; }
}

// DTO thống kê phục vụ chẩn đoán dữ liệu phiên đọc bài.
public sealed class DiagnosticsStatsResult
{
    // Tổng số session hiện có trong MongoDB.
    public long TotalSessionsInMongo { get; init; }

    // Số session thuộc tài khoản test để tách khỏi dữ liệu thật.
    public long TestUserSessions { get; init; }

    // Mẫu dữ liệu thô để hỗ trợ điều tra nhanh khi cần.
    public List<string> SampleDataRaw { get; init; } = new();
}
