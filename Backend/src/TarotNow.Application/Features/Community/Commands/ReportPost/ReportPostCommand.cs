/*
 * ===================================================================
 * FILE: ReportPostCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.ReportPost
 * ===================================================================
 * MỤC ĐÍCH:
 *   Tạo một báo cáo vi phạm đối với bài viết cộng đồng (Post).
 *   Sử dụng chung cơ sở hạ tầng của IReportRepository.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ReportPost;

public class ReportPostCommand : IRequest<ReportDto>
{
    public string PostId { get; set; } = string.Empty;
    public Guid ReporterId { get; set; }
    public string ReasonCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ReportPostCommandHandler : IRequestHandler<ReportPostCommand, ReportDto>
{
    private readonly IReportRepository _reportRepo;
    private readonly ICommunityPostRepository _postRepo;

    public ReportPostCommandHandler(IReportRepository reportRepo, ICommunityPostRepository postRepo)
    {
        _reportRepo = reportRepo;
        _postRepo = postRepo;
    }

    public async Task<ReportDto> Handle(ReportPostCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");

        var reporterId = request.ReporterId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != reporterId)
            throw new ForbiddenException("Bạn không có quyền báo cáo bài viết riêng tư này.");

        if (post.AuthorId == reporterId)
            throw new BadRequestException("Bạn không thể tự báo cáo bài viết của chính mình.");

        if (!CommunityModuleConstants.SupportedReportReasonCodes.Contains(request.ReasonCode))
            throw new BadRequestException("Mã lý do không hợp lệ.");

        if (string.IsNullOrWhiteSpace(request.Description) || request.Description.Length < 10)
            throw new BadRequestException("Mô tả vi phạm phải có ít nhất 10 ký tự.");

        // 2. Tạo biên bản Tố cáo
        var report = new ReportDto
        {
            ReporterId = request.ReporterId.ToString(),
            TargetType = "post", // Target là "post" để tách biệt với "message", "conversation"
            TargetId = request.PostId,
            Reason = $"[{request.ReasonCode}] {request.Description}",
            Status = PostReportStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }
}
