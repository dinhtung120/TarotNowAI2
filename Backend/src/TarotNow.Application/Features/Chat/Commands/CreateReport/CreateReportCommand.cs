using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

/// <summary>
/// Command tạo báo cáo vi phạm.
/// </summary>
public class CreateReportCommand : IRequest<ReportDto>
{
    /// <summary>UUID người báo cáo.</summary>
    public Guid ReporterId { get; set; }

    /// <summary>Loại: message | conversation | user.</summary>
    public string TargetType { get; set; } = string.Empty;

    /// <summary>ID đối tượng bị báo cáo.</summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>ObjectId conversation liên quan (nếu có).</summary>
    public string? ConversationRef { get; set; }

    /// <summary>Lý do báo cáo.</summary>
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Handler tạo report.
/// Validate: reason phải có nội dung, target_type hợp lệ.
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
        // 1. Validate target_type
        var validTypes = new[] { "message", "conversation", "user" };
        if (!validTypes.Contains(request.TargetType))
            throw new BadRequestException("Loại đối tượng không hợp lệ. Chỉ chấp nhận: message, conversation, user.");

        // 2. Validate reason
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
            throw new BadRequestException("Lý do báo cáo phải có ít nhất 10 ký tự.");

        // 3. Tạo report
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

        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }
}
