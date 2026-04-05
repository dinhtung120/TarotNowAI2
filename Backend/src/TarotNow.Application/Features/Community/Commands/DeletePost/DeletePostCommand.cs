/*
 * ===================================================================
 * FILE: DeletePostCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Commands.DeletePost
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xoá mềm bài viết cộng đồng.
 *   Author tự xoá hoặc Admin can thiệp xoá.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.DeletePost;

public class DeletePostCommand : IRequest<bool>
{
    public string PostId { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
    public string RequesterRole { get; set; } = string.Empty;
}

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
{
    private readonly ICommunityPostRepository _postRepo;

    public DeletePostCommandHandler(ICommunityPostRepository postRepo)
    {
        _postRepo = postRepo;
    }

    public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var existingPost = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (existingPost == null || existingPost.IsDeleted)
            throw new NotFoundException("Không tìm thấy bài viết hoặc bài viết đã bị xoá.");

        // Rule: Author hoặc Admin mới được xóa
        bool isAuthor = existingPost.AuthorId == request.RequesterId.ToString();
        bool isAdmin = request.RequesterRole == UserRole.Admin;

        if (!isAuthor && !isAdmin)
            throw new ForbiddenException("Bạn không có quyền xoá bài viết này.");

        return await _postRepo.SoftDeleteAsync(request.PostId, request.RequesterId.ToString(), cancellationToken);
    }
}
