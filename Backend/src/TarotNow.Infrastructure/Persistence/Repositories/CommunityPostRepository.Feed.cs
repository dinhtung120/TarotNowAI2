using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CommunityPostRepository
{
    public async Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page,
        int pageSize,
        string? viewerId = null,
        string? authorId = null,
        string? visibility = null,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildFeedFilter(viewerId, authorId, visibility);
        var skip = Math.Max(page - 1, 0) * pageSize;
        var total = await _context.CommunityPosts.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.CommunityPosts.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(MapToDto), total);
    }

    private static FilterDefinition<CommunityPostDocument> BuildFeedFilter(string? viewerId, string? authorId, string? visibility)
    {
        var builder = Builders<CommunityPostDocument>.Filter;
        var filter = builder.Eq(x => x.IsDeleted, false);

        if (!string.IsNullOrWhiteSpace(authorId))
        {
            filter &= builder.Eq(x => x.AuthorId, authorId);
        }

        if (!string.IsNullOrWhiteSpace(visibility))
        {
            return filter & BuildVisibilityFilter(builder, visibility, viewerId);
        }

        return filter & BuildDefaultVisibilityFilter(builder, viewerId);
    }

    private static FilterDefinition<CommunityPostDocument> BuildVisibilityFilter(
        FilterDefinitionBuilder<CommunityPostDocument> builder,
        string visibility,
        string? viewerId)
    {
        if (!string.Equals(visibility, PostVisibility.Private, StringComparison.Ordinal))
        {
            return builder.Eq(x => x.Visibility, visibility);
        }

        if (string.IsNullOrWhiteSpace(viewerId))
        {
            return builder.And(
                builder.Eq(x => x.Visibility, PostVisibility.Private),
                builder.Eq(x => x.AuthorId, "BLOCK_UNAUTHENTICATED"));
        }

        return builder.And(
            builder.Eq(x => x.Visibility, PostVisibility.Private),
            builder.Eq(x => x.AuthorId, viewerId));
    }

    private static FilterDefinition<CommunityPostDocument> BuildDefaultVisibilityFilter(
        FilterDefinitionBuilder<CommunityPostDocument> builder,
        string? viewerId)
    {
        var publicFilter = builder.Eq(x => x.Visibility, PostVisibility.Public);
        if (string.IsNullOrWhiteSpace(viewerId))
        {
            return publicFilter;
        }

        var privateMine = builder.And(
            builder.Eq(x => x.Visibility, PostVisibility.Private),
            builder.Eq(x => x.AuthorId, viewerId));
        return builder.Or(publicFilter, privateMine);
    }
}
