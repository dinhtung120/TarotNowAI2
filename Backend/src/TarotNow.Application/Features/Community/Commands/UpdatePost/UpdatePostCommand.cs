

using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Community.Commands.UpdatePost;

// Command cập nhật nội dung bài viết community.
public class UpdatePostCommand : IRequest<bool>
{
    // Định danh bài viết cần cập nhật.
    public string PostId { get; set; } = string.Empty;

    // Định danh tác giả yêu cầu cập nhật.
    public Guid AuthorId { get; set; }

    // Nội dung mới của bài viết.
    public string Content { get; set; } = string.Empty;
}

// Handler xử lý cập nhật bài viết.
public class UpdatePostCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<UpdatePostCommandHandlerRequestedDomainEvent>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityMediaAttachmentService _communityMediaAttachmentService;

    /// <summary>
    /// Khởi tạo handler update post.
    /// Luồng xử lý: nhận post repository để kiểm tra quyền sở hữu và cập nhật nội dung.
    /// </summary>
    public UpdatePostCommandHandlerRequestedDomainEventHandler(
        ICommunityPostRepository postRepo,
        ICommunityMediaAttachmentService communityMediaAttachmentService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _postRepo = postRepo;
        _communityMediaAttachmentService = communityMediaAttachmentService;
    }

    /// <summary>
    /// Xử lý command cập nhật bài viết.
    /// Luồng xử lý: validate content, kiểm tra post tồn tại/chưa xóa, kiểm tra quyền tác giả, rồi lưu nội dung mới đã trim.
    /// </summary>
    public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
        {
            // Nội dung phải có ý nghĩa và trong ngưỡng cho phép.
            throw new BadRequestException("Nội dung bài viết không được trống và tối đa 5000 ký tự.");
        }

        var existingPost = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (existingPost == null || existingPost.IsDeleted)
        {
            // Bài viết không tồn tại hoặc đã bị xóa không thể cập nhật.
            throw new NotFoundException("Không tìm thấy bài viết hoặc bài viết đã bị xoá.");
        }

        if (existingPost.AuthorId != request.AuthorId.ToString())
        {
            // Chỉ tác giả bài viết mới được phép sửa nội dung.
            throw new ForbiddenException("Bạn không có quyền sửa bài viết này.");
        }

        var normalizedContent = request.Content.Trim();
        var updated = await _postRepo.UpdateContentAsync(request.PostId, normalizedContent, cancellationToken);
        if (!updated)
        {
            return false;
        }

        await _communityMediaAttachmentService.ReconcileForExistingEntityAsync(
            request.AuthorId,
            "post",
            request.PostId,
            normalizedContent,
            cancellationToken);

        return true;
    }

    protected override async Task HandleDomainEventAsync(
        UpdatePostCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
