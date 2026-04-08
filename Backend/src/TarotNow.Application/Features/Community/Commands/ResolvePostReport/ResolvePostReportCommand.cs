using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

// Command xử lý kết quả moderation cho report bài viết.
public class ResolvePostReportCommand : IRequest<bool>
{
    // Định danh report cần xử lý.
    public string ReportId { get; set; } = string.Empty;

    // Định danh admin thực hiện xử lý.
    public Guid AdminId { get; set; }

    // Kết quả moderation (warn/remove/freeze/no_action).
    public string Result { get; set; } = string.Empty;

    // Ghi chú xử lý từ admin (tùy chọn).
    public string? AdminNote { get; set; }
}

// Handler xử lý resolve report community post.
public class ResolvePostReportCommandHandler : IRequestHandler<ResolvePostReportCommand, bool>
{
    private readonly ICommunityPostRepository _postRepository;
    private readonly IReportRepository _reportRepository;

    /// <summary>
    /// Khởi tạo handler resolve post report.
    /// Luồng xử lý: nhận repository post/report để xác thực report và thực thi hành động moderation.
    /// </summary>
    public ResolvePostReportCommandHandler(
        ICommunityPostRepository postRepository,
        IReportRepository reportRepository)
    {
        _postRepository = postRepository;
        _reportRepository = reportRepository;
    }

    /// <summary>
    /// Xử lý command resolve report.
    /// Luồng xử lý: validate result, tải report hợp lệ/pending, xử lý action remove post nếu cần, rồi cập nhật trạng thái resolved.
    /// </summary>
    public async Task<bool> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
    {
        ValidateModerationResult(request.Result);

        var report = await _reportRepository.GetByIdAsync(request.ReportId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy report.");

        if (string.Equals(report.TargetType, "post", StringComparison.OrdinalIgnoreCase) == false)
        {
            // Chỉ xử lý report thuộc community post trong handler này.
            throw new BadRequestException("Report không thuộc community post.");
        }

        if (report.Status != PostReportStatus.Pending && report.Status != PostReportStatus.Processing)
        {
            // Report đã xử lý trước đó thì chặn thao tác resolve lặp.
            throw new BadRequestException("Report đã được xử lý trước đó.");
        }

        if (request.Result == ModerationResult.RemovePost)
        {
            // Khi chọn RemovePost, phải gỡ bài trước khi chốt report.
            await RemovePostOrThrowAsync(report.TargetId, request.AdminId, cancellationToken);
        }

        var resolved = await _reportRepository.ResolveAsync(
            reportId: request.ReportId,
            status: PostReportStatus.Resolved,
            result: request.Result,
            resolvedBy: request.AdminId.ToString(),
            adminNote: request.AdminNote,
            cancellationToken: cancellationToken);

        if (!resolved)
        {
            // Edge case repository không cập nhật được trạng thái report.
            throw new BadRequestException("Không thể cập nhật trạng thái report.");
        }

        return true;
    }

    /// <summary>
    /// Kiểm tra kết quả moderation đầu vào có hợp lệ hay không.
    /// Luồng xử lý: so sánh với tập kết quả được hệ thống hỗ trợ.
    /// </summary>
    private static void ValidateModerationResult(string result)
    {
        var validResults = new HashSet<string>(StringComparer.Ordinal)
        {
            ModerationResult.Warn,
            ModerationResult.RemovePost,
            ModerationResult.FreezeAccount,
            ModerationResult.NoAction
        };

        if (validResults.Contains(result) == false)
        {
            throw new BadRequestException("Kết quả xử lý không hợp lệ.");
        }
    }

    /// <summary>
    /// Gỡ bài viết theo quyết định moderation, ném lỗi nếu không tìm thấy bài.
    /// Luồng xử lý: gọi soft-delete với ngữ cảnh admin và xác nhận thao tác thành công.
    /// </summary>
    private async Task RemovePostOrThrowAsync(
        string postId,
        Guid adminId,
        CancellationToken cancellationToken)
    {
        var removed = await _postRepository.SoftDeleteAsync(postId, adminId.ToString(), cancellationToken);
        if (!removed)
        {
            // Edge case report trỏ tới bài đã bị xóa hoặc không còn tồn tại.
            throw new NotFoundException("Không tìm thấy bài viết để gỡ.");
        }
    }
}
