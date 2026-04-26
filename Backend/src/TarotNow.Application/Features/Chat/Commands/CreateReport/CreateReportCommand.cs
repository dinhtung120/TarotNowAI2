using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

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
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _chatMessageRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Khởi tạo handler create report.
    /// Luồng xử lý: nhận report repository để persist dữ liệu báo cáo vi phạm.
    /// </summary>
    public CreateReportCommandHandler(
        IReportRepository reportRepo,
        IConversationRepository conversationRepo,
        IChatMessageRepository chatMessageRepo,
        IUserRepository userRepo)
    {
        _reportRepo = reportRepo;
        _conversationRepo = conversationRepo;
        _chatMessageRepo = chatMessageRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Xử lý command tạo báo cáo.
    /// Luồng xử lý: kiểm tra target type và reason, dựng report dto trạng thái pending, lưu DB và trả kết quả.
    /// </summary>
    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var normalizedTargetType = request.TargetType?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!ReportTargetTypes.All.Contains(normalizedTargetType))
        {
            // Rule business: chỉ cho phép báo cáo 3 loại đối tượng đã hỗ trợ moderation.
            throw new BadRequestException("Loại đối tượng không hợp lệ. Chỉ chấp nhận: message, conversation, user.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Length < 10)
        {
            // Bắt buộc lý do đủ dài để moderation có thông tin xử lý thực tế.
            throw new BadRequestException("Lý do báo cáo phải có ít nhất 10 ký tự.");
        }

        var normalizedConversationRef = await ValidateTargetAndResolveConversationRefAsync(
            request.ReporterId,
            normalizedTargetType,
            request.TargetId,
            request.ConversationRef,
            cancellationToken);

        var report = new ReportDto
        {
            ReporterId = request.ReporterId.ToString(),
            TargetType = normalizedTargetType,
            TargetId = request.TargetId,
            ConversationRef = normalizedConversationRef,
            Reason = request.Reason,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        // Persist báo cáo mới trước khi trả về để caller có trạng thái đồng bộ.
        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }

    private async Task<string?> ValidateTargetAndResolveConversationRefAsync(
        Guid reporterId,
        string targetType,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        var normalizedTargetId = NormalizeRequiredTargetId(targetId);
        return targetType switch
        {
            ReportTargetTypes.Message => await ResolveMessageTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                conversationRef,
                cancellationToken),
            ReportTargetTypes.Conversation => await ResolveConversationTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                cancellationToken),
            ReportTargetTypes.User => await ResolveUserTargetConversationAsync(
                reporterId,
                normalizedTargetId,
                conversationRef,
                cancellationToken),
            _ => throw new BadRequestException("Loại đối tượng không hợp lệ.")
        };
    }

    private static string NormalizeRequiredTargetId(string targetId)
    {
        var normalizedTargetId = targetId?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(normalizedTargetId))
        {
            return normalizedTargetId;
        }

        throw new BadRequestException("TargetId là bắt buộc.");
    }

    private async Task<string> ResolveMessageTargetConversationAsync(
        Guid reporterId,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        var message = await _chatMessageRepo.GetByIdAsync(targetId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy message cần report.");
        if (IsConversationRefMismatch(conversationRef, message.ConversationId))
        {
            throw new BadRequestException("ConversationRef không khớp với message được report.");
        }

        var conversation = await _conversationRepo.GetByIdAsync(message.ConversationId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy conversation của message được report.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        return conversation.Id;
    }

    private async Task<string> ResolveConversationTargetConversationAsync(
        Guid reporterId,
        string targetId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(targetId, cancellationToken)
            ?? throw new BadRequestException("Không tìm thấy conversation cần report.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        return conversation.Id;
    }

    private async Task<string?> ResolveUserTargetConversationAsync(
        Guid reporterId,
        string targetId,
        string? conversationRef,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(targetId, out var targetUserId))
        {
            throw new BadRequestException("TargetId user không hợp lệ.");
        }

        var user = await _userRepo.GetByIdAsync(targetUserId, cancellationToken);
        if (user == null)
        {
            throw new BadRequestException("Không tìm thấy user cần report.");
        }

        if (string.IsNullOrWhiteSpace(conversationRef))
        {
            return null;
        }

        var conversation = await _conversationRepo.GetByIdAsync(conversationRef.Trim(), cancellationToken)
            ?? throw new BadRequestException("ConversationRef không tồn tại.");
        EnsureReporterHasConversationAccess(reporterId, conversation);
        return conversation.Id;
    }

    private static bool IsConversationRefMismatch(string? conversationRef, string conversationId)
    {
        return !string.IsNullOrWhiteSpace(conversationRef)
               && !string.Equals(conversationRef.Trim(), conversationId, StringComparison.Ordinal);
    }

    private static void EnsureReporterHasConversationAccess(Guid reporterId, ConversationDto conversation)
    {
        var reporter = reporterId.ToString();
        if (conversation.UserId == reporter || conversation.ReaderId == reporter)
        {
            return;
        }

        throw new BadRequestException("Bạn không có quyền report đối tượng ngoài phạm vi truy cập.");
    }
}
