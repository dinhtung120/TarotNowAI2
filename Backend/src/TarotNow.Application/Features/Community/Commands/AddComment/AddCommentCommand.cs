using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.AddComment;

// Command thêm bình luận vào community post.
public class AddCommentCommand : IRequest<CommunityCommentDto>
{
    // Định danh bài viết cần bình luận.
    public required string PostId { get; set; }

    // Định danh tác giả bình luận.
    public required Guid AuthorId { get; set; }

    // Nội dung bình luận.
    public required string Content { get; set; }
}

// Handler xử lý thêm bình luận cho community post.
public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommunityCommentDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityCommentRepository _commentRepo;
    private readonly IUserRepository _userRepo;

    /// <summary>
    /// Khởi tạo handler add comment.
    /// Luồng xử lý: nhận repository bài viết, bình luận và user để kiểm tra quyền + tạo comment đầy đủ thông tin tác giả.
    /// </summary>
    public AddCommentCommandHandler(
        ICommunityPostRepository postRepo,
        ICommunityCommentRepository commentRepo,
        IUserRepository userRepo)
    {
        _postRepo = postRepo;
        _commentRepo = commentRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Xử lý command thêm bình luận.
    /// Luồng xử lý: validate nội dung, kiểm tra bài viết tồn tại và quyền truy cập, tải thông tin user, tạo comment mới rồi tăng comments count của post.
    /// </summary>
    public async Task<CommunityCommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            // Bình luận rỗng không có giá trị nghiệp vụ nên bị từ chối.
            throw new ValidationException("Bình luận không được để trống.");
        }

        if (request.Content.Length > 1000)
        {
            // Giới hạn độ dài để bảo đảm UX và tránh payload quá lớn.
            throw new ValidationException("Bình luận không được vượt quá 1000 ký tự.");
        }

        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken)
            ?? throw new NotFoundException($"Không tìm thấy bài viết ID {request.PostId}.");

        if (post.Visibility == PostVisibility.Private
            && post.AuthorId != request.AuthorId.ToString())
        {
            // Bài viết private chỉ cho chủ bài viết tương tác bình luận.
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

        // Cập nhật bộ đếm bình luận sau khi tạo comment thành công.
        await _postRepo.IncrementCommentsCountAsync(request.PostId, 1, cancellationToken);

        return createdComment;
    }
}
