using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ResolvePostReport;

public class ResolvePostReportCommand : IRequest<bool>
{
    public string ReportId { get; set; } = string.Empty;
    public Guid AdminId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
}

public class ResolvePostReportCommandHandler : IRequestHandler<ResolvePostReportCommand, bool>
{
    private readonly ICommunityPostRepository _postRepository;
    private readonly IReportRepository _reportRepository;

    public ResolvePostReportCommandHandler(
        ICommunityPostRepository postRepository,
        IReportRepository reportRepository)
    {
        _postRepository = postRepository;
        _reportRepository = reportRepository;
    }

    public async Task<bool> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
    {
        ValidateModerationResult(request.Result);

        var report = await _reportRepository.GetByIdAsync(request.ReportId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy report.");

        if (string.Equals(report.TargetType, "post", StringComparison.OrdinalIgnoreCase) == false)
        {
            throw new BadRequestException("Report không thuộc community post.");
        }

        if (report.Status != PostReportStatus.Pending && report.Status != PostReportStatus.Processing)
        {
            throw new BadRequestException("Report đã được xử lý trước đó.");
        }

        if (request.Result == ModerationResult.RemovePost)
        {
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
            throw new BadRequestException("Không thể cập nhật trạng thái report.");
        }

        return true;
    }

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

    private async Task RemovePostOrThrowAsync(
        string postId,
        Guid adminId,
        CancellationToken cancellationToken)
    {
        var removed = await _postRepository.SoftDeleteAsync(postId, adminId.ToString(), cancellationToken);
        if (!removed)
        {
            throw new NotFoundException("Không tìm thấy bài viết để gỡ.");
        }
    }
}
