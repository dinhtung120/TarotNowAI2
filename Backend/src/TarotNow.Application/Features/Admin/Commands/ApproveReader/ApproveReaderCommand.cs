using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

// Command duyệt hoặc từ chối đơn đăng ký reader.
public class ApproveReaderCommand : IRequest<bool>
{
    // Định danh đơn đăng ký reader cần xử lý.
    public string RequestId { get; set; } = string.Empty;

    // Hành động xử lý (approve/reject).
    public string Action { get; set; } = string.Empty;

    // Ghi chú của admin cho quyết định xử lý.
    public string? AdminNote { get; set; }

    // Định danh admin thực hiện thao tác duyệt.
    public Guid AdminId { get; set; }
}
