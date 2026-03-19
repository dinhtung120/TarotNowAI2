/*
 * ===================================================================
 * FILE: ListDepositsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListDeposits
 * ===================================================================
 * MỤC ĐÍCH:
 *   Query + Response DTO + Item DTO cho admin XEM DANH SÁCH ĐƠN NẠP TIỀN.
 *   Admin dùng trang này để:
 *   - Xem đơn nạp tiền đang chờ xử lý (Pending)
 *   - Duyệt hoặc từ chối đơn (ProcessDeposit)
 *   - Theo dõi lịch sử nạp tiền
 *
 * [JsonPropertyName]: Attribute kiểm soát tên field khi serialize thành JSON.
 *   C# conventions: PascalCase (Status)
 *   JSON conventions: camelCase (status)
 *   [JsonPropertyName("status")]: đổi "Status" → "status" trong response JSON.
 * ===================================================================
 */

using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

/// <summary>
/// Query lấy danh sách đơn nạp tiền (có phân trang + lọc).
/// </summary>
public class ListDepositsQuery : IRequest<ListDepositsResponse>
{
    /// <summary>Trang hiện tại (1-indexed). Mặc định trang 1.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Số item mỗi trang. Mặc định 20.</summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Lọc theo trạng thái: "Pending", "Success", "Failed" (nullable = lấy tất cả).
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// Response DTO chứa danh sách đơn nạp tiền + tổng số.
/// </summary>
public class ListDepositsResponse
{
    /// <summary>Danh sách đơn nạp tiền (đã phân trang).</summary>
    [JsonPropertyName("deposits")] // JSON: "deposits" thay vì "Deposits"
    public IEnumerable<DepositDto> Deposits { get; set; } = new List<DepositDto>();

    /// <summary>Tổng số đơn nạp (tất cả trang, dùng cho pagination UI).</summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

/// <summary>
/// DTO đại diện cho 1 đơn nạp tiền trong danh sách.
/// </summary>
public class DepositDto
{
    /// <summary>UUID đơn nạp tiền.</summary>
    [JsonPropertyName("id")]
    public System.Guid Id { get; set; }

    /// <summary>UUID user nạp tiền.</summary>
    [JsonPropertyName("userId")]
    public System.Guid UserId { get; set; }

    /// <summary>
    /// Tên user — DENORMALIZED (sao chép từ users table).
    /// Giúp admin xem nhanh "ai nạp" mà không cần click vào chi tiết.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>Số tiền VND nạp (tiền thật). Ví dụ: 100000 = 100.000 VND.</summary>
    [JsonPropertyName("amountVnd")]
    public long AmountVnd { get; set; }

    /// <summary>Số diamond tương ứng (tính theo tỷ giá). Ví dụ: 100 diamond.</summary>
    [JsonPropertyName("diamondAmount")]
    public long DiamondAmount { get; set; }

    /// <summary>
    /// Trạng thái: "Pending" (chờ), "Success" (thành công), "Failed" (thất bại).
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Mã giao dịch từ payment gateway (VNPay, MoMo,...). Null nếu chưa có.</summary>
    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    /// <summary>Thời điểm tạo đơn nạp.</summary>
    [JsonPropertyName("createdAt")]
    public System.DateTime CreatedAt { get; set; }
}
