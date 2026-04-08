using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

// Command tạo báo cáo vi phạm cho message/conversation/user.
public class CreateReportCommand : IRequest<ReportDto>
{
    // Định danh người gửi báo cáo.
    public Guid ReporterId { get; set; }

    // Loại đối tượng bị báo cáo.
    public string TargetType { get; set; } = string.Empty;

    // Định danh đối tượng bị báo cáo.
    public string TargetId { get; set; } = string.Empty;

    // Conversation tham chiếu (nếu có).
    public string? ConversationRef { get; set; }

    // Lý do báo cáo.
    public string Reason { get; set; } = string.Empty;
}

// Handler tạo report và lưu vào repository.
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportDto>
{
    private readonly IReportRepository _reportRepo;

    /// <summary>
    /// Khởi tạo handler create report.
    /// Luồng xử lý: nhận report repository để persist dữ liệu báo cáo vi phạm.
    /// </summary>
    public CreateReportCommandHandler(IReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    /// <summary>
    /// Xử lý command tạo báo cáo.
    /// Luồng xử lý: kiểm tra target type và reason, dựng report dto trạng thái pending, lưu DB và trả kết quả.
    /// </summary>
    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var validTypes = new[] { "message", "conversation", "user" };
        if (!validTypes.Contains(request.TargetType))
        {
            // Rule business: chỉ cho phép báo cáo 3 loại đối tượng đã hỗ trợ moderation.
            throw new BadRequestException("Loại đối tượng không hợp lệ. Chỉ chấp nhận: message, conversation, user.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
        {
            // Bắt buộc lý do đủ dài để moderation có thông tin xử lý thực tế.
            throw new BadRequestException("Lý do báo cáo phải có ít nhất 10 ký tự.");
        }

        var report = new ReportDto
        {
            ReporterId = request.ReporterId.ToString(),
            TargetType = request.TargetType,
            TargetId = request.TargetId,
            ConversationRef = request.ConversationRef,
            Reason = request.Reason,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        // Persist báo cáo mới trước khi trả về để caller có trạng thái đồng bộ.
        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }
}
