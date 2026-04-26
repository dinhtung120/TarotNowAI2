

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Commands.DeletePost;

// Command xóa mềm community post.
public class DeletePostCommand : IRequest<bool>
{
    // Định danh bài viết cần xóa.
    public string PostId { get; set; } = string.Empty;

    // Định danh người yêu cầu xóa.
    public Guid RequesterId { get; set; }

    // Vai trò người yêu cầu (dùng cho quyền admin).
    public string RequesterRole { get; set; } = string.Empty;
}

// Handler xử lý xóa mềm bài viết.
public class DeletePostCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DeletePostCommandHandlerRequestedDomainEvent>
{
    private readonly ICommunityPostRepository _postRepo;

    /// <summary>
    /// Khởi tạo handler delete post.
    /// Luồng xử lý: nhận post repository để kiểm tra quyền và thực hiện soft-delete.
    /// </summary>
    public DeletePostCommandHandlerRequestedDomainEventHandler(
        ICommunityPostRepository postRepo,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _postRepo = postRepo;
    }

    /// <summary>
    /// Xử lý command xóa bài viết.
    /// Luồng xử lý: kiểm tra post tồn tại/chưa xóa, kiểm tra quyền tác giả hoặc admin, rồi thực hiện soft-delete.
    /// </summary>
    public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var existingPost = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (existingPost == null || existingPost.IsDeleted)
        {
            // Edge case: bài viết không tồn tại hoặc đã bị xóa trước đó.
            throw new NotFoundException("Không tìm thấy bài viết hoặc bài viết đã bị xoá.");
        }

        // Phân tách rõ quyền xóa: tác giả hoặc admin.
        bool isAuthor = existingPost.AuthorId == request.RequesterId.ToString();
        bool isAdmin = request.RequesterRole == UserRole.Admin;

        if (!isAuthor && !isAdmin)
        {
            // Chặn người dùng không đủ quyền thực hiện thao tác xóa.
            throw new ForbiddenException("Bạn không có quyền xoá bài viết này.");
        }

        return await _postRepo.SoftDeleteAsync(request.PostId, request.RequesterId.ToString(), cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        DeletePostCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
