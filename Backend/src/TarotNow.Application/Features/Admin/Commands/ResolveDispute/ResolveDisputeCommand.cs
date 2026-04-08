using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

// Command xử lý tranh chấp tài chính của một question item.
public class ResolveDisputeCommand : IRequest<bool>
{
    // Định danh question item đang ở trạng thái dispute.
    public Guid ItemId { get; set; }

    // Định danh admin thực hiện xử lý tranh chấp.
    public Guid AdminId { get; set; }

    // Hành động xử lý tranh chấp (release/refund/split).
    public string Action { get; set; } = string.Empty;

    // Tỷ lệ phần trăm chuyển cho reader khi action là split.
    public int? SplitPercentToReader { get; set; }

    // Ghi chú xử lý tranh chấp của admin.
    public string? AdminNote { get; set; }
}
