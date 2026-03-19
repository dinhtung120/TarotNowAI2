/*
 * ===================================================================
 * FILE: ApproveReaderCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Commands.ApproveReader
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command cho Admin PHÊ DUYỆT hoặc TỪ CHỐI đơn xin làm Reader.
 *   Đây là phần "data" của CQRS — chứa thông tin cần thiết cho thao tác.
 *   Handler (logic) nằm ở file ApproveReaderCommandHandler.cs.
 *
 * LUỒNG NGHIỆP VỤ:
 *   User nộp đơn → Admin xem → Admin approve/reject → Thay đổi hệ thống
 *
 * NẾU APPROVE:
 *   1. MongoDB: cập nhật reader_requests.status = "approved"
 *   2. PostgreSQL: cập nhật users.role = "tarot_reader"
 *   3. MongoDB: tạo document mới trong reader_profiles (hồ sơ reader)
 *
 * NẾU REJECT:
 *   1. MongoDB: cập nhật reader_requests.status = "rejected"
 *   2. PostgreSQL: giữ nguyên users.role = "user" (không đổi)
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

/// <summary>
/// Command cho Admin phê duyệt hoặc từ chối đơn xin Reader.
/// IRequest<bool>: trả về true nếu thành công.
/// </summary>
public class ApproveReaderCommand : IRequest<bool>
{
    /// <summary>
    /// ObjectId string của document reader_requests trong MongoDB.
    /// Admin chọn đơn từ danh sách → gửi ID để xử lý.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>
    /// Hành động: "approve" (phê duyệt) hoặc "reject" (từ chối).
    /// Validate trong Handler (không dùng enum để giữ API đơn giản).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú của admin — giải thích lý do quyết định (tùy chọn).
    /// Ví dụ: "Hồ sơ không đủ chứng minh kinh nghiệm" (khi reject).
    /// Lưu vào reader_requests để user biết lý do.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// UUID admin thực hiện thao tác — lấy từ JWT claims trong Controller.
    /// Ghi vào audit trail: biết AI đã duyệt/từ chối.
    /// </summary>
    public Guid AdminId { get; set; }
}
