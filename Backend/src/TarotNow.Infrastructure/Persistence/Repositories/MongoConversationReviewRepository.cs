using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý review reader theo conversation trên MongoDB.
/// </summary>
public sealed class MongoConversationReviewRepository : IConversationReviewRepository
{
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository conversation review.
    /// </summary>
    public MongoConversationReviewRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ConversationReviewDto?> GetByConversationAndUserAsync(
        string conversationId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationReviewDocument>.Filter.And(
            Builders<ConversationReviewDocument>.Filter.Eq(x => x.ConversationId, conversationId),
            Builders<ConversationReviewDocument>.Filter.Eq(x => x.UserId, userId));

        var doc = await _context.ConversationReviews
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <inheritdoc />
    public async Task<bool> TryAddAsync(ConversationReviewDto review, CancellationToken cancellationToken = default)
    {
        var document = ToDocument(review);
        try
        {
            await _context.ConversationReviews.InsertOneAsync(document, cancellationToken: cancellationToken);
            review.Id = document.Id;
            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }

    private static ConversationReviewDto ToDto(ConversationReviewDocument document)
    {
        return new ConversationReviewDto
        {
            Id = document.Id,
            ConversationId = document.ConversationId,
            UserId = document.UserId,
            ReaderId = document.ReaderId,
            Rating = document.Rating,
            Comment = document.Comment,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
    }

    private static ConversationReviewDocument ToDocument(ConversationReviewDto dto)
    {
        return new ConversationReviewDocument
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            ConversationId = dto.ConversationId,
            UserId = dto.UserId,
            ReaderId = dto.ReaderId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
