using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

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

    // Draft id để map asset upload trước khi comment thật được tạo.
    public string? ContextDraftId { get; set; }
}

// Handler xử lý thêm bình luận cho community post.
public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommunityCommentDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler add comment.
    /// Luồng xử lý: command chỉ publish domain event theo rule event-only.
    /// </summary>
    public AddCommentCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command thêm bình luận.
    /// Luồng xử lý: validate input, publish event request, rồi dựng response từ payload đã được handler xử lý.
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

        var domainEvent = new CommunityCommentAddRequestedDomainEvent
        {
            PostId = request.PostId,
            AuthorId = request.AuthorId,
            Content = request.Content.Trim(),
            ContextDraftId = string.IsNullOrWhiteSpace(request.ContextDraftId) ? null : request.ContextDraftId.Trim()
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        if (string.IsNullOrWhiteSpace(domainEvent.CreatedCommentId))
        {
            throw new BadRequestException("Không thể tạo bình luận.");
        }

        return new CommunityCommentDto
        {
            Id = domainEvent.CreatedCommentId,
            PostId = domainEvent.PostId,
            AuthorId = request.AuthorId.ToString(),
            AuthorDisplayName = domainEvent.AuthorDisplayName,
            AuthorAvatarUrl = domainEvent.AuthorAvatarUrl,
            Content = domainEvent.Content,
            CreatedAt = domainEvent.CreatedAtUtc,
            MediaAttachStatus = string.IsNullOrWhiteSpace(domainEvent.MediaAttachStatus)
                ? MediaUploadConstants.EntityMediaAttachStatusNone
                : domainEvent.MediaAttachStatus,
            MediaAttachLastError = null
        };
    }
}
