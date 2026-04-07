

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateReport;

public class CreateReportCommand : IRequest<ReportDto>
{
        public Guid ReporterId { get; set; }

        public string TargetType { get; set; } = string.Empty;

        public string TargetId { get; set; } = string.Empty;

        public string? ConversationRef { get; set; }

        public string Reason { get; set; } = string.Empty;
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportDto>
{
    private readonly IReportRepository _reportRepo;

    public CreateReportCommandHandler(IReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        
        
        
        var validTypes = new[] { "message", "conversation", "user" };
        if (!validTypes.Contains(request.TargetType))
            throw new BadRequestException("Loại đối tượng không hợp lệ. Chỉ chấp nhận: message, conversation, user.");

        
        
        
        
        
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
            throw new BadRequestException("Lý do báo cáo phải có ít nhất 10 ký tự.");

        
        
        
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
