

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
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
        
        if (!CommunityModuleConstants.SupportedReactionTypes.Contains(request.ReactionType))
            throw new BadRequestException("Loại biểu cảm không hợp lệ.");

        
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");

        var userIdStr = request.UserId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != userIdStr)
            throw new ForbiddenException("Bạn không có quyền tương tác với bài viết riêng tư này.");

        
        var existingReaction = await _reactionRepo.GetAsync(request.PostId, userIdStr, cancellationToken);

        if (existingReaction == null)
            return await HandleCaseAddNewReactionAsync(request, userIdStr, cancellationToken);
            
        if (existingReaction.Type == request.ReactionType)
            return await HandleCaseRemoveReactionAsync(request, userIdStr, cancellationToken);
            
        return await HandleCaseSwapReactionAsync(request, existingReaction.Type, userIdStr, cancellationToken);
    }

    
    
    
    
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
