using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SubmitConversationReview;

/// <summary>
/// Workflow handler xử lý submit review sau khi conversation completed.
/// </summary>
public sealed class SubmitConversationReviewCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SubmitConversationReviewCommandHandlerRequestedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IConversationReviewRepository _conversationReviewRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo workflow handler submit review.
    /// </summary>
    public SubmitConversationReviewCommandHandlerRequestedDomainEventHandler(
        IConversationRepository conversationRepository,
        IConversationReviewRepository conversationReviewRepository,
        IReaderProfileRepository readerProfileRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _conversationReviewRepository = conversationReviewRepository;
        _readerProfileRepository = readerProfileRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        SubmitConversationReviewCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await HandleAsync(domainEvent.Command, cancellationToken);
    }

    private async Task<ConversationReviewSubmitResult> HandleAsync(
        SubmitConversationReviewCommand request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToString();
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        if (conversation.UserId != userId)
        {
            throw new BadRequestException("Chỉ User của cuộc trò chuyện mới được gửi đánh giá.");
        }

        if (conversation.Status != ConversationStatus.Completed)
        {
            throw new BadRequestException("Chỉ có thể đánh giá khi cuộc trò chuyện đã hoàn thành.");
        }

        var existingReview = await _conversationReviewRepository.GetByConversationAndUserAsync(
            request.ConversationId,
            userId,
            cancellationToken);
        if (existingReview != null)
        {
            return ToResult(existingReview);
        }

        var review = new ConversationReviewDto
        {
            ConversationId = request.ConversationId,
            UserId = userId,
            ReaderId = conversation.ReaderId,
            Rating = request.Rating,
            Comment = NormalizeComment(request.Comment),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _conversationReviewRepository.TryAddAsync(review, cancellationToken);
        if (created == false)
        {
            var duplicate = await _conversationReviewRepository.GetByConversationAndUserAsync(
                request.ConversationId,
                userId,
                cancellationToken);
            return duplicate == null ? ToResult(review) : ToResult(duplicate);
        }

        await UpdateReaderProfileRatingAsync(review.ReaderId, review.Rating, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(
                conversation.Id,
                "review_submitted",
                review.CreatedAt),
            cancellationToken);

        return ToResult(review);
    }

    private async Task UpdateReaderProfileRatingAsync(
        string readerId,
        int rating,
        CancellationToken cancellationToken)
    {
        var profile = await _readerProfileRepository.GetByUserIdAsync(readerId, cancellationToken);
        if (profile == null)
        {
            return;
        }

        var total = profile.TotalReviews;
        var updatedTotal = total + 1;
        var updatedAverage = ((profile.AvgRating * total) + rating) / updatedTotal;
        profile.TotalReviews = updatedTotal;
        profile.AvgRating = Math.Round(updatedAverage, 2, MidpointRounding.AwayFromZero);
        await _readerProfileRepository.UpdateAsync(profile, cancellationToken);
    }

    private static string? NormalizeComment(string? comment)
    {
        var normalized = comment?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static ConversationReviewSubmitResult ToResult(ConversationReviewDto review)
    {
        return new ConversationReviewSubmitResult
        {
            ConversationId = review.ConversationId,
            ReaderId = review.ReaderId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
}
