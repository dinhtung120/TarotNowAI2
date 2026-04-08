using MediatR;
using System.Text.Json.Serialization;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

// Query phân trang danh sách lệnh nạp tiền cho admin.
public class ListDepositsQuery : IRequest<ListDepositsResponse>
{
    // Trang hiện tại (1-based).
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;

    // Bộ lọc trạng thái lệnh nạp (tùy chọn).
    public string? Status { get; set; }
}

// Kết quả trả về cho truy vấn danh sách lệnh nạp.
public class ListDepositsResponse
{
    // Danh sách lệnh nạp của trang hiện tại.
    [JsonPropertyName("deposits")]
    public IEnumerable<DepositDto> Deposits { get; set; } = new List<DepositDto>();

    // Tổng số bản ghi theo bộ lọc.
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

// DTO hiển thị một lệnh nạp tiền trong màn quản trị.
public class DepositDto
{
    // Định danh lệnh nạp.
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    // Định danh user tạo lệnh nạp.
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    // Username hiển thị để admin tra cứu nhanh.
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    // Số tiền nạp theo VND.
    [JsonPropertyName("amountVnd")]
    public long AmountVnd { get; set; }

    // Số kim cương quy đổi tương ứng.
    [JsonPropertyName("diamondAmount")]
    public long DiamondAmount { get; set; }

    // Trạng thái xử lý lệnh nạp.
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    // Mã giao dịch đối soát từ cổng thanh toán/admin.
    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    // Thời điểm tạo lệnh nạp.
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
