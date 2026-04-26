using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler tạo comment community theo event request.
/// </summary>
public sealed class CommunityCommentAddRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CommunityCommentAddRequestedDomainEvent>
{
    private readonly ICommunityPostRepository _postRepository;
    private readonly ICommunityCommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler tạo comment community.
    /// </summary>
    public CommunityCommentAddRequestedDomainEventHandler(
        ICommunityPostRepository postRepository,
        ICommunityCommentRepository commentRepository,
        IUserRepository userRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        CommunityCommentAddRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var post = await LoadAccessiblePostAsync(domainEvent, cancellationToken);
        var user = await _userRepository.GetByIdAsync(domainEvent.AuthorId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");

        var requiresMediaAttach = RequiresMediaAttach(domainEvent.ContextDraftId, domainEvent.Content);
        var createdComment = await CreateCommentAsync(domainEvent, user, requiresMediaAttach, cancellationToken);

        await _postRepository.IncrementCommentsCountAsync(post.Id, 1, cancellationToken);
        HydrateResultPayload(domainEvent, createdComment);
        await PublishMediaAttachRequestIfNeededAsync(domainEvent, createdComment, requiresMediaAttach, cancellationToken);
    }

    private async Task<CommunityPostDto> LoadAccessiblePostAsync(
        CommunityCommentAddRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(domainEvent.PostId, cancellationToken)
            ?? throw new NotFoundException($"Không tìm thấy bài viết ID {domainEvent.PostId}.");

        if (post.Visibility == PostVisibility.Private && post.AuthorId != domainEvent.AuthorId.ToString())
        {
            throw new ForbiddenException("Bạn không có quyền bình luận bài viết riêng tư này.");
        }

        return post;
    }

    private Task<CommunityCommentDto> CreateCommentAsync(
        CommunityCommentAddRequestedDomainEvent domainEvent,
        TarotNow.Domain.Entities.User user,
        bool requiresMediaAttach,
        CancellationToken cancellationToken)
    {
        var mediaAttachStatus = requiresMediaAttach
            ? MediaUploadConstants.EntityMediaAttachStatusPending
            : MediaUploadConstants.EntityMediaAttachStatusNone;

        return _commentRepository.AddCommentAsync(
            new CommunityCommentDto
            {
                PostId = domainEvent.PostId,
                AuthorId = user.Id.ToString(),
                AuthorDisplayName = user.DisplayName,
                AuthorAvatarUrl = user.AvatarUrl,
                Content = domainEvent.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                MediaAttachStatus = mediaAttachStatus,
                MediaAttachLastError = null
            },
            cancellationToken);
    }

    private static void HydrateResultPayload(
        CommunityCommentAddRequestedDomainEvent domainEvent,
        CommunityCommentDto createdComment)
    {
        domainEvent.CreatedCommentId = createdComment.Id;
        domainEvent.AuthorDisplayName = createdComment.AuthorDisplayName;
        domainEvent.AuthorAvatarUrl = createdComment.AuthorAvatarUrl;
        domainEvent.CreatedAtUtc = createdComment.CreatedAt;
        domainEvent.MediaAttachStatus = createdComment.MediaAttachStatus;
    }

    private async Task PublishMediaAttachRequestIfNeededAsync(
        CommunityCommentAddRequestedDomainEvent domainEvent,
        CommunityCommentDto createdComment,
        bool requiresMediaAttach,
        CancellationToken cancellationToken)
    {
        if (!requiresMediaAttach)
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(
            new CommunityMediaAttachRequestedDomainEvent
            {
                OwnerUserId = domainEvent.AuthorId,
                ContextType = MediaUploadConstants.ContextComment,
                ContextDraftId = domainEvent.ContextDraftId,
                ContextEntityId = createdComment.Id,
                MarkdownContent = createdComment.Content
            },
            cancellationToken);
    }

    private static bool RequiresMediaAttach(string? contextDraftId, string markdownContent)
    {
        if (!string.IsNullOrWhiteSpace(contextDraftId))
        {
            return true;
        }

        return MarkdownImageLinkExtractor.ExtractUrls(markdownContent).Count > 0;
    }
}
