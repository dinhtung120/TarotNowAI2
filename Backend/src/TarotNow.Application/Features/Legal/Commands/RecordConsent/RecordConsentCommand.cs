using System;
using MediatR;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

// Command ghi nhận việc người dùng đã đồng ý một tài liệu pháp lý tại thời điểm hiện tại.
public class RecordConsentCommand : IRequest<bool>
{
    // Định danh người dùng thực hiện đồng ý điều khoản.
    public Guid UserId { get; set; }

    // Loại tài liệu pháp lý cần ghi nhận (TOS, PrivacyPolicy, AiDisclaimer).
    public string DocumentType { get; set; } = string.Empty;

    // Phiên bản tài liệu mà người dùng đã đồng ý.
    public string Version { get; set; } = string.Empty;

    // Địa chỉ IP tại thời điểm ghi nhận consent để phục vụ audit.
    public string IpAddress { get; set; } = string.Empty;

    // User-Agent tại thời điểm ghi nhận consent để truy vết thiết bị.
    public string UserAgent { get; set; } = string.Empty;
}
