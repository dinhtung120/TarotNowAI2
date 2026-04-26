using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Community.Commands.CreatePost;

// Command tạo bài viết community mới.
public class CreatePostCommand : IRequest<CommunityPostDto>
{
    // Định danh tác giả bài viết.
    public Guid AuthorId { get; set; }

    // Nội dung bài viết.
    public string Content { get; set; } = string.Empty;

    // Mức hiển thị của bài viết (public/private).
    public string Visibility { get; set; } = PostVisibility.Public;

    // Draft id để map asset upload trước khi post thật được tạo.
    public string? ContextDraftId { get; set; }
}

// Handler xử lý tạo community post.
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CommunityPostDto>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler create post.
    /// Luồng xử lý: command chỉ publish domain event theo rule event-only.
    /// </summary>
    public CreatePostCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command tạo bài viết.
    /// Luồng xử lý: validate input, publish event request, rồi dựng response từ payload đã được handler xử lý.
    /// </summary>
    public async Task<CommunityPostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
        {
            // Rule nghiệp vụ: bài viết phải có nội dung và nằm trong giới hạn chiều dài.
            throw new BadRequestException("Nội dung bài viết không được trống và tối đa 5000 ký tự.");
        }

        var validVisibilities = new[] { PostVisibility.Public, PostVisibility.Private };
        if (!validVisibilities.Contains(request.Visibility))
        {
            // Chặn visibility ngoài tập hỗ trợ để tránh dữ liệu sai schema hiển thị.
            throw new BadRequestException("Quyền riêng tư không hợp lệ.");
        }

        var domainEvent = new CommunityPostCreateRequestedDomainEvent
        {
            AuthorId = request.AuthorId,
            Content = request.Content.Trim(),
            Visibility = request.Visibility,
            ContextDraftId = string.IsNullOrWhiteSpace(request.ContextDraftId) ? null : request.ContextDraftId.Trim()
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        if (string.IsNullOrWhiteSpace(domainEvent.CreatedPostId))
        {
            throw new BadRequestException("Không thể tạo bài viết.");
        }

        return new CommunityPostDto
        {
            Id = domainEvent.CreatedPostId,
            AuthorId = request.AuthorId.ToString(),
            AuthorDisplayName = domainEvent.AuthorDisplayName,
            AuthorAvatarUrl = domainEvent.AuthorAvatarUrl,
            Content = domainEvent.Content,
            Visibility = domainEvent.Visibility,
            CreatedAt = domainEvent.CreatedAtUtc,
            ReactionsCount = new Dictionary<string, int>(),
            TotalReactions = 0,
            CommentsCount = 0,
            IsDeleted = false,
            MediaAttachStatus = string.IsNullOrWhiteSpace(domainEvent.MediaAttachStatus)
                ? MediaUploadConstants.EntityMediaAttachStatusNone
                : domainEvent.MediaAttachStatus,
            MediaAttachLastError = null
        };
    }
}
