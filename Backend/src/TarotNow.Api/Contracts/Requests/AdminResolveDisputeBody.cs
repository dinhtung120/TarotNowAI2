namespace TarotNow.Api.Contracts.Requests;

// Payload để admin xử lý một dispute.
public class AdminResolveDisputeBody
{
    // Hành động xử lý dispute (ví dụ: refund, release, split).
    public string Action { get; set; } = string.Empty;

    // Tỷ lệ phần trăm chia cho reader khi action là split.
    public int? SplitPercentToReader { get; set; }

    // Ghi chú nội bộ của admin để truy vết quyết định xử lý.
    public string? AdminNote { get; set; }
}
