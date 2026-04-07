using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace TarotNow.Application.Features.Community.Commands.AddComment;

public class AddCommentCommand : IRequest<CommunityCommentDto>
{
    public required string PostId { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Content { get; set; }
}

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommunityCommentDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityCommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;

    public AddCommentCommandHandler(
        ICommunityPostRepository postRepo,
        ICommunityCommentRepository commentRepo,
        IUserRepository userRepo)
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
        _userRepo = userRepo;
    }

    public async Task<CommunityCommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            throw new ValidationException("Bình luận không được để trống.");
        
        if (request.Content.Length > 1000)
            throw new ValidationException("Bình luận không được vượt quá 1000 ký tự.");

        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken)
            ?? throw new NotFoundException($"Không tìm thấy bài viết ID {request.PostId}.");

        
        if (post.Visibility == PostVisibility.Private 
            && post.AuthorId != request.AuthorId.ToString())
        {
            throw new ForbiddenException("Bạn không có quyền bình luận bài viết riêng tư này.");
        }

        var user = await _userRepo.GetByIdAsync(request.AuthorId)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        var commentDto = new CommunityCommentDto
        {
            PostId = request.PostId,
            AuthorId = user.Id.ToString(),
            AuthorDisplayName = user.DisplayName,
            AuthorAvatarUrl = user.AvatarUrl,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        var createdComment = await _commentRepo.AddCommentAsync(commentDto, cancellationToken);

        
        await _postRepo.IncrementCommentsCountAsync(request.PostId, 1, cancellationToken);

        return createdComment;
    }
}
