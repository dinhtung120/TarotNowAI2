/*
 * ===================================================================
 * FILE: CheckConsentQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Legal.Queries.CheckConsent
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh Cảnh Sát Giao Thông. Mỗi khi User mở Frontend App lên, 
 *   App sẽ gọi ngầm API này để hỏi Backend "Ông khách này đã Ký Đủ giấy tờ chưa?".
 * 
 * NOTE:
 *   Nếu chưa ký (Vd: Backend vừa ra Luật mới V2.0) -> Frontend sẽ khóa màn hình, 
 *   ném ra cái Popup bắt User kéo xuống tận cùng đọc và Bấm "Tôi Giao Phó Đời Mình..." thì mới cho xài App tiếp.
 * ===================================================================
 */

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Legal.Queries.CheckConsent;

public class CheckConsentQuery : IRequest<CheckConsentResponse>
{
    public Guid UserId { get; set; }
    
    // Nếu để trống 2 dòng dưới -> Hệ thống sẽ TỰ AUTO KIỂM TRA TOÀN BỘ 3 VĂN BẢN (Theo cấu hình Version Mới Nhất lưu trong appsettings.json).
    public string? DocumentType { get; set; }
    public string? Version { get; set; }
}

public class CheckConsentResponse
{
    /// <summary>True = Đã ngoan ngoãn ký đủ hết. False = Bị thiếu nợ văn bản, chặn cửa!</summary>
    public bool IsFullyConsented { get; set; }
    
    /// <summary>Trả về danh sách những Loại Văn Bản Mới chưa đóng dấu (Ví dụ: Trả về ["TOS", "AiDisclaimer"]). Front-end dựa vào đây để hiển thị đúng Popup.</summary>
    public List<string> PendingDocuments { get; set; } = new();
}
