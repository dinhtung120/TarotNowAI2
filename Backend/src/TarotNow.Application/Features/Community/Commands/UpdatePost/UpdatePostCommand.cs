/*
 * ===================================================================
 * FILE: UpdatePostCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.UpdatePost
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cập nhật (sửa) nội dung bài viết cộng đồng (Chỉ Author mới được sửa).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Commands.UpdatePost;

public class UpdatePostCommand : IRequest<bool>
{
    public string PostId { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
{
    private readonly ICommunityPostRepository _postRepo;

    public UpdatePostCommandHandler(ICommunityPostRepository postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
            throw new BadRequestException("Nội dung bài viết không được trống và tối đa 5000 ký tự.");

        var existingPost = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (existingPost == null || existingPost.IsDeleted)
            throw new NotFoundException("Không tìm thấy bài viết hoặc bài viết đã bị xoá.");

        // Quyền lực tối thượng: Chỉ chủ nhân văn bản mới được phép múa bút sửa đổi
        if (existingPost.AuthorId != request.AuthorId.ToString())
            throw new ForbiddenException("Bạn không có quyền sửa bài viết này.");

        return await _postRepo.UpdateContentAsync(request.PostId, request.Content.Trim(), cancellationToken);
    }
}
