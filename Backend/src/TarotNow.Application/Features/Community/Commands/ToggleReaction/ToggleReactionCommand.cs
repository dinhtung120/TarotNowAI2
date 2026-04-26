

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ToggleReaction;

// Command thêm/gỡ/đổi reaction cho community post.
public class ToggleReactionCommand : IRequest<bool>
{
    // Định danh bài viết cần tương tác.
    public string PostId { get; set; } = string.Empty;

    // Định danh user thực hiện reaction.
    public Guid UserId { get; set; }

    // Loại reaction mục tiêu.
    public string ReactionType { get; set; } = string.Empty;
}

// Handler xử lý toggle reaction theo ngữ cảnh hiện tại của user.
public class ToggleReactionCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ToggleReactionCommandHandlerRequestedDomainEvent>
{
    private readonly ICommunityReactionRepository _reactionRepo;
    private readonly ICommunityPostRepository _postRepo;

    /// <summary>
    /// Khởi tạo handler toggle reaction.
    /// Luồng xử lý: nhận repository reaction/post để kiểm tra trạng thái hiện tại và cập nhật bộ đếm reaction tương ứng.
    /// </summary>
    public ToggleReactionCommandHandlerRequestedDomainEventHandler(
        ICommunityReactionRepository reactionRepo,
        ICommunityPostRepository postRepo,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _reactionRepo = reactionRepo;
        _postRepo = postRepo;
    }

    /// <summary>
    /// Xử lý command toggle reaction.
    /// Luồng xử lý: validate reaction type + quyền truy cập post, đọc reaction hiện tại và phân nhánh add/remove/swap.
    /// </summary>
    public async Task<bool> Handle(ToggleReactionCommand request, CancellationToken cancellationToken)
    {
        if (!CommunityModuleConstants.SupportedReactionTypes.Contains(request.ReactionType))
        {
            // Chỉ cho phép reaction thuộc whitelist module community.
            throw new BadRequestException("Loại biểu cảm không hợp lệ.");
        }

        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
        {
            // Bài viết không tồn tại hoặc đã xóa thì không thể tương tác reaction.
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");
        }

        var userIdStr = request.UserId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != userIdStr)
        {
            // Post private chỉ cho chủ bài viết tương tác trong phạm vi hiện tại.
            throw new ForbiddenException("Bạn không có quyền tương tác với bài viết riêng tư này.");
        }

        var existingReaction = await _reactionRepo.GetAsync(request.PostId, userIdStr, cancellationToken);

        if (existingReaction == null)
        {
            // Chưa có reaction trước đó: thêm reaction mới.
            return await HandleCaseAddNewReactionAsync(request, userIdStr, cancellationToken);
        }

        if (existingReaction.Type == request.ReactionType)
        {
            // Chọn lại cùng loại reaction: hiểu là thao tác gỡ reaction.
            return await HandleCaseRemoveReactionAsync(request, userIdStr, cancellationToken);
        }

        // Đã có reaction khác loại: chuyển sang reaction mới.
        return await HandleCaseSwapReactionAsync(request, existingReaction.Type, userIdStr, cancellationToken);
    }

    /// <summary>
    /// Nhánh thêm reaction mới khi user chưa từng react post này.
    /// Luồng xử lý: tạo reaction, thêm vào repository và tăng bộ đếm reaction tương ứng khi insert thành công.
    /// </summary>
    private async Task<bool> HandleCaseAddNewReactionAsync(ToggleReactionCommand request, string userIdStr, CancellationToken cancellationToken)
    {
        var newReaction = new CommunityReactionDto
        {
            PostId = request.PostId,
            UserId = userIdStr,
            Type = request.ReactionType,
            CreatedAt = DateTime.UtcNow
        };

        var added = await _reactionRepo.AddOrIgnoreAsync(newReaction, cancellationToken);
        if (added)
        {
            // Chỉ tăng bộ đếm khi thêm reaction thành công (tránh lệch count do race condition).
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// Nhánh gỡ reaction khi user bấm lại đúng reaction hiện có.
    /// Luồng xử lý: xóa reaction và giảm bộ đếm tương ứng khi thao tác remove thành công.
    /// </summary>
    private async Task<bool> HandleCaseRemoveReactionAsync(ToggleReactionCommand request, string userIdStr, CancellationToken cancellationToken)
    {
        var removed = await _reactionRepo.RemoveAsync(request.PostId, userIdStr, cancellationToken);
        if (removed)
        {
            // Chỉ giảm count khi xóa thực sự thành công.
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, -1, cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// Nhánh đổi reaction từ loại cũ sang loại mới.
    /// Luồng xử lý: cập nhật type reaction của user rồi đồng bộ bộ đếm cũ (-1) và mới (+1).
    /// </summary>
    private async Task<bool> HandleCaseSwapReactionAsync(ToggleReactionCommand request, string oldType, string userIdStr, CancellationToken cancellationToken)
    {
        var swapped = await _reactionRepo.UpdateTypeAsync(request.PostId, userIdStr, request.ReactionType, cancellationToken);
        if (swapped)
        {
            // Cập nhật song song hai bộ đếm để tổng reaction không bị lệch.
            await _postRepo.IncrementReactionCountAsync(request.PostId, oldType, -1, cancellationToken);
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);
        }

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        ToggleReactionCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
