

using MediatR;
using System;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ReportPost;

// Command báo cáo vi phạm cho community post.
public class ReportPostCommand : IRequest<ReportDto>
{
    // Định danh bài viết bị báo cáo.
    public string PostId { get; set; } = string.Empty;

    // Định danh người báo cáo.
    public Guid ReporterId { get; set; }

    // Mã lý do báo cáo.
    public string ReasonCode { get; set; } = string.Empty;

    // Mô tả chi tiết vi phạm.
    public string Description { get; set; } = string.Empty;
}

// Handler xử lý tạo report cho bài viết community.
public class ReportPostCommandHandler : IRequestHandler<ReportPostCommand, ReportDto>
{
    private readonly IReportRepository _reportRepo;
    private readonly ICommunityPostRepository _postRepo;

    /// <summary>
    /// Khởi tạo handler report post.
    /// Luồng xử lý: nhận repository report và post để validate mục tiêu rồi ghi report.
    /// </summary>
    public ReportPostCommandHandler(IReportRepository reportRepo, ICommunityPostRepository postRepo)
    {
        _reportRepo = reportRepo;
        _postRepo = postRepo;
    }

    /// <summary>
    /// Xử lý command báo cáo bài viết.
    /// Luồng xử lý: kiểm tra post hợp lệ, kiểm tra quyền report, validate reason/description, tạo report pending và lưu repository.
    /// </summary>
    public async Task<ReportDto> Handle(ReportPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
        {
            // Bài viết không tồn tại/đã xóa thì không thể báo cáo.
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");
        }

        var reporterId = request.ReporterId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != reporterId)
        {
            // Post private chỉ cho chủ sở hữu thao tác báo cáo trong bối cảnh hiện tại.
            throw new ForbiddenException("Bạn không có quyền báo cáo bài viết riêng tư này.");
        }

        if (post.AuthorId == reporterId)
        {
            // Business rule: cấm tự report bài viết của chính mình.
            throw new BadRequestException("Bạn không thể tự báo cáo bài viết của chính mình.");
        }

        if (!CommunityModuleConstants.SupportedReportReasonCodes.Contains(request.ReasonCode))
        {
            // Reason code phải nằm trong whitelist để phục vụ moderation analytics nhất quán.
            throw new BadRequestException("Mã lý do không hợp lệ.");
        }

        if (string.IsNullOrWhiteSpace(request.Description) || request.Description.Length < 10)
        {
            // Description tối thiểu 10 ký tự để đủ ngữ cảnh xử lý moderation.
            throw new BadRequestException("Mô tả vi phạm phải có ít nhất 10 ký tự.");
        }

        var report = new ReportDto
        {
            ReporterId = request.ReporterId.ToString(),
            // Gắn target type cố định để routing về luồng moderation community post.
            TargetType = "post",
            TargetId = request.PostId,
            Reason = $"[{request.ReasonCode}] {request.Description}",
            Status = PostReportStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _reportRepo.AddAsync(report, cancellationToken);
        return report;
    }
}
