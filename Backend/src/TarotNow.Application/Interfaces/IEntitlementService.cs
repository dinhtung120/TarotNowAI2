/*
 * ===================================================================
 * FILE: IEntitlementService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Dịch Vụ Điều Phối (Service) lõi dành cho Đặc Quyền (Entitlement).
 *   Lý do thiết kế: Hàm consume khá phức tạp (bắt transaction, load lock, trừ lần lượt, ghi log),
 *   nên được gom vào một Service tiện dụng cho tất cả Handler khác gọi thay vì viết đi viết lại.
 *   Nó cũng bao phủ lấy Cache Dữ Liệu bằng Redis cho việc Check nhanh số lượng Bốc Của Khách.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Trả Về Kết Quả Báo Cáo Tính Mạng Của Quyền Lợi.
/// </summary>
public record EntitlementBalanceDto(
    string EntitlementKey,
    int DailyQuotaTotal,
    int UsedToday,
    int RemainingToday
);

/// <summary>
/// Trả kết quả lệnh Trừ Thẳng (Consume).
/// </summary>
public record EntitlementConsumeResult(
    bool Success, 
    string Message
);

/// <summary>
/// Dịch Vụ Core Quản Lý Bóc Lá Rút Thưởng Quyền Lợi.
/// </summary>
public interface IEntitlementService
{
    /// <summary>
    /// Ép Ngực Trừ Đi 1 Vé Vào Cửa Từ Rổ.
    /// Trả về True/False xem có đớn được 1 miếng Bánh Miễn Phí Nào Không.
    /// Nếu Hết Hạn Ngạch -> Trả Về Lỗi Failed.
    /// Tự Động Xử Lý Khóa Dòng Transaction + Idempotency Bảo Lãnh Không Hút Nhầm Dù Bắn Hai Lệnh.
    /// </summary>
    Task<EntitlementConsumeResult> ConsumeAsync(
        Guid userId, 
        string entitlementKey, 
        string referenceSource,
        string referenceId, 
        string idempotencyKey, 
        CancellationToken ct);
    
    /// <summary>
    /// Dò Khám Bảng Cân Đối Hiện Tại Xem Khách Còn Mã Giảm Không Nhé? (Sẽ Có Redis Chặn Thống Kê Nhanh Đây).
    /// KHÔNG Trừ Tiền Ở Hàm Này.
    /// </summary>
    Task<EntitlementBalanceDto> GetBalanceAsync(
        Guid userId, 
        string entitlementKey, 
        CancellationToken ct);
    
    /// <summary>
    /// Gói Hộp Tất Cả Số Dư Mọi Quyền Của Khách Trả Về (Phục vụ Tải Trang Lịch Sử Mua).
    /// </summary>
    Task<List<EntitlementBalanceDto>> GetAllBalancesAsync(
        Guid userId, 
        CancellationToken ct);
}
