/*
 * ===================================================================
 * FILE: ToggleReactionCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.ToggleReaction
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý logic thả cảm xúc (Like/Love/Haha...) cho một bài viết.
 *   - Nếu chưa react: Thêm mới.
 *   - Nếu đã react cùng loại: Hủy react.
 *   - Nếu đã react nhưng khác loại: Đổi react.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.ToggleReaction;

public class ToggleReactionCommand : IRequest<bool>
{
    public string PostId { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string ReactionType { get; set; } = string.Empty;
}

public class ToggleReactionCommandHandler : IRequestHandler<ToggleReactionCommand, bool>
{
    private readonly ICommunityReactionRepository _reactionRepo;
    private readonly ICommunityPostRepository _postRepo;

    public ToggleReactionCommandHandler(ICommunityReactionRepository reactionRepo, ICommunityPostRepository postRepo)
    {
        _reactionRepo = reactionRepo;
        _postRepo = postRepo;
    }

    public async Task<bool> Handle(ToggleReactionCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Reaction Type
        var validTypes = new[] { 
            ReactionType.Like, ReactionType.Love, ReactionType.Insightful, 
            ReactionType.Haha, ReactionType.Sad 
        };
        
        if (!validTypes.Contains(request.ReactionType))
            throw new BadRequestException("Loại biểu cảm không hợp lệ.");

        // 2. Validate Post
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");

        // 3. Xử lý Logic Toggle
        var userIdStr = request.UserId.ToString();
        var existingReaction = await _reactionRepo.GetAsync(request.PostId, userIdStr, cancellationToken);

        if (existingReaction == null)
            return await HandleCaseAddNewReactionAsync(request, userIdStr, cancellationToken);
            
        if (existingReaction.Type == request.ReactionType)
            return await HandleCaseRemoveReactionAsync(request, userIdStr, cancellationToken);
            
        return await HandleCaseSwapReactionAsync(request, existingReaction.Type, userIdStr, cancellationToken);
    }

    // TODO: [TECH-DEBT] Wrap AddOrIgnoreAsync + IncrementReactionCountAsync
    // trong MongoDB Session Transaction để chống race condition khi
    // 2 request đồng thời gửi đến. Unique Index chặn được duplicate insert,
    // nhưng counter có thể bị lệch nếu 1 insert bị reject mà Inc đã chạy.
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
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);
            
        return true;
    }

    private async Task<bool> HandleCaseRemoveReactionAsync(ToggleReactionCommand request, string userIdStr, CancellationToken cancellationToken)
    {
        var removed = await _reactionRepo.RemoveAsync(request.PostId, userIdStr, cancellationToken);
        if (removed)
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, -1, cancellationToken);
            
        return true;
    }

    private async Task<bool> HandleCaseSwapReactionAsync(ToggleReactionCommand request, string oldType, string userIdStr, CancellationToken cancellationToken)
    {
        var swapped = await _reactionRepo.UpdateTypeAsync(request.PostId, userIdStr, request.ReactionType, cancellationToken);
        if (swapped)
        {
            await _postRepo.IncrementReactionCountAsync(request.PostId, oldType, -1, cancellationToken);
            await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);
        }
        return true;
    }
}
