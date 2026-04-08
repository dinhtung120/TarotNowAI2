
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// DTO số dư entitlement để phản ánh hạn mức và mức đã dùng trong ngày.
public record EntitlementBalanceDto(
    string EntitlementKey,
    int DailyQuotaTotal,
    int UsedToday,
    int RemainingToday
);

// Kết quả tiêu thụ entitlement cho biết thành công và thông điệp nghiệp vụ.
public record EntitlementConsumeResult(
    bool Success,
    string Message
);

// Contract cấp phát/tiêu thụ entitlement để kiểm soát quota theo gói dịch vụ.
public interface IEntitlementService
{
    /// <summary>
    /// Tiêu thụ một đơn vị entitlement khi người dùng thực hiện hành động cần quota.
    /// Luồng xử lý: nhận request tiêu thụ, kiểm tra hạn mức và trả kết quả thành công/thất bại.
    /// </summary>
    Task<EntitlementConsumeResult> ConsumeAsync(EntitlementConsumeRequest request, CancellationToken ct);

    /// <summary>
    /// Lấy số dư entitlement cụ thể để hiển thị quyền lợi còn lại của người dùng.
    /// Luồng xử lý: tính tổng quota, số đã dùng và phần còn lại theo entitlementKey.
    /// </summary>
    Task<EntitlementBalanceDto> GetBalanceAsync(
        Guid userId,
        string entitlementKey,
        CancellationToken ct);

    /// <summary>
    /// Lấy toàn bộ số dư entitlement của người dùng để tổng hợp dashboard quyền lợi.
    /// Luồng xử lý: duyệt các entitlement khả dụng và trả danh sách balance tương ứng.
    /// </summary>
    Task<List<EntitlementBalanceDto>> GetAllBalancesAsync(
        Guid userId,
        CancellationToken ct);
}

// Request tiêu thụ entitlement để đảm bảo idempotency và truy vết theo nguồn nghiệp vụ.
public sealed record EntitlementConsumeRequest(
    Guid UserId,
    string EntitlementKey,
    string ReferenceSource,
    string ReferenceId,
    string IdempotencyKey);
