using MediatR;
using System;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
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
public class ResolvePostReportCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ResolvePostReportCommandHandlerRequestedDomainEvent>
{
    private readonly ICommunityPostRepository _postRepository;
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;

    /// <summary>
    /// Khởi tạo handler resolve post report.
    /// Luồng xử lý: nhận repository post/report để xác thực report và thực thi hành động moderation.
    /// </summary>
    public ResolvePostReportCommandHandlerRequestedDomainEventHandler(
        ICommunityPostRepository postRepository,
        IReportRepository reportRepository,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _postRepository = postRepository;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
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

        var isRemovePostAction = string.Equals(request.Result, ModerationResult.RemovePost, StringComparison.Ordinal);
        var isFreezeAccountAction = string.Equals(request.Result, ModerationResult.FreezeAccount, StringComparison.Ordinal);

        CommunityPostDto? reportedPost = null;
        if (isRemovePostAction || isFreezeAccountAction)
        {
            reportedPost = await GetPostOrThrowAsync(report.TargetId, cancellationToken);
        }

        Guid? lockedUserId = null;
        if (isFreezeAccountAction && reportedPost is not null)
        {
            lockedUserId = await LockReportedUserAndRevokeSessionsAsync(reportedPost.AuthorId, cancellationToken);
        }

        var resolved = await _reportRepository.ResolvePostReportWithPostMutationAsync(
            new PostReportResolutionMutation
            {
                ReportId = request.ReportId,
                PostId = report.TargetId,
                RemovePost = isRemovePostAction,
                Status = PostReportStatus.Resolved,
                Result = request.Result,
                ResolvedBy = request.AdminId.ToString(),
                AdminNote = request.AdminNote,
                DeletedBy = request.AdminId.ToString()
            },
            cancellationToken);

        if (!resolved)
        {
            if (lockedUserId.HasValue)
            {
                await TryRollbackUserLockAsync(lockedUserId.Value, cancellationToken);
            }

            // Edge case repository không cập nhật được trạng thái report.
            throw new BadRequestException("Không thể cập nhật trạng thái report.");
        }

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        ResolvePostReportCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
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

    private async Task<CommunityPostDto> GetPostOrThrowAsync(
        string postId,
        CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(postId, cancellationToken);
        if (post == null || post.IsDeleted)
        {
            // Edge case report trỏ tới bài đã bị xóa hoặc không còn tồn tại.
            throw new NotFoundException("Không tìm thấy bài viết để gỡ.");
        }

        return post;
    }

    private async Task<Guid> LockReportedUserAndRevokeSessionsAsync(
        string authorId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(authorId, out var targetUserId))
        {
            throw new BadRequestException("Target user không hợp lệ cho action freeze_account.");
        }

        var user = await _userRepository.GetByIdAsync(targetUserId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy user để khóa tài khoản.");
        user.Lock();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _refreshTokenRepository.RevokeAllByUserIdAsync(targetUserId, cancellationToken);
        await _authSessionRepository.RevokeAllByUserAsync(targetUserId, cancellationToken);
        return targetUserId;
    }

    private async Task TryRollbackUserLockAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return;
            }

            user.Unlock();
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch
        {
            // Best-effort rollback khi resolve report thất bại sau bước lock.
        }
    }
}
