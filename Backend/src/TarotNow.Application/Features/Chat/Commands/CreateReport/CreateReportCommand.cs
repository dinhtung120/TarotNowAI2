/*
 * ===================================================================
 * FILE: CreateReportCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.CreateReport
 * ===================================================================
 * MỤC ĐÍCH:
 *   File chứa CẢ Command và CommandHandler cho chức năng REPORT VI PHẠM (TỐ CÁO).
 *   Trong ứng dụng Chat, tính năng tố cáo (Spam/Hate speech/Nhạy cảm) là cực kỳ 
 *   quan trọng để Admin can thiệp khoá tài khoản/hoàn tiền.
 *
 * SCOPE BÁO CÁO:
 *   Hệ thống thiết kế Đa Năng có thể báo cáo:
 *   - Lời nói (Message cụ thể).
 *   - Môi trường chat (Cả phòng Conversation).
 *   - Đứng từ User (Tố cáo User cụ thể).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

/// <summary>
/// Gói biểu mẫu Đơn Phản Ánh do hệ thống Gửi tới Admin Dashboard.
/// DTO chuẩn bị cho Report.
/// </summary>
public class CreateReportCommand : IRequest<ReportDto>
{
    /// <summary>Người gõ đơn (Luôn lấy từ Context JWT Token của Header cho chính xác tuyệt đối).</summary>
    public Guid ReporterId { get; set; }

    /// <summary>Con trỏ phân biệt hình thái báo cáo (message | conversation | user).</summary>
    public string TargetType { get; set; } = string.Empty;

    /// <summary>Cái ID trỏ đến thực thể bị báo cáo (Đoạn mã MongoDB ObjectID).</summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>Gắn kết đường dẫn vụ án mạng vào đúng Box Chat nào (Để Admin trích xuất xem trộm cho nhanh).</summary>
    public string? ConversationRef { get; set; }

    /// <summary>Diễn đạt nội dung vì sao tố cáo (Lý do lừa tiền, chửi thề...).</summary>
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Handler đứng ra thụ lý Đơn Phản Ánh và Lưu vào Kho lưu trữ (Reports) dưới CSDL.
/// </summary>
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportDto>
{
    private readonly IReportRepository _reportRepo;

    public CreateReportCommandHandler(IReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        // -------------------------------------------------------------
        //  1. CỔNG CHẶN TYPE: Admin chỉ cung cấp 3 loại thủ tục tố cáo này thôi.
        // -------------------------------------------------------------
        var validTypes = new[] { "message", "conversation", "user" };
        if (!validTypes.Contains(request.TargetType))
            throw new BadRequestException("Loại đối tượng không hợp lệ. Chỉ chấp nhận: message, conversation, user.");

        // -------------------------------------------------------------
        //  2. CỔNG CHẶN NỘI DUNG RÁC:
        //  Tránh việc người dùng ức chế tố cáo bằng các chữ aaa, ddd...
        //  Yêu cầu bắt buộc viết tối thiểu 10 kí tự.
        // -------------------------------------------------------------
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
            throw new BadRequestException("Lý do báo cáo phải có ít nhất 10 ký tự.");

        // -------------------------------------------------------------
        //  3. LẬP BIÊN BẢN (KHỞI TẠO REPORT DTO)
        // -------------------------------------------------------------
        var report = new ReportDto
        {
            ReporterId = request.ReporterId.ToString(),
            TargetType = request.TargetType,
            TargetId = request.TargetId,
            ConversationRef = request.ConversationRef,
            Reason = request.Reason,
            Status = "pending", // Đợi Nhân viên duyệt
            CreatedAt = DateTime.UtcNow
        };

        // Lưu vào lòng NoSQL
        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }
}
