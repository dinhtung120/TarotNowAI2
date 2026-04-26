using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý nghiệp vụ engagement của bài viết cộng đồng.
public partial class CommunityPostRepository
{
    /// <summary>
    /// Tăng/giảm số reaction của bài viết theo loại reaction.
    /// Luồng xử lý: cập nhật đồng thời bucket reactions_count[type] và total_reactions để giữ thống kê nhất quán.
    /// </summary>
    public async Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update.Combine(
            Builders<CommunityPostDocument>.Update.Inc($"reactions_count.{reactionType}", delta),
            Builders<CommunityPostDocument>.Update.Inc(x => x.TotalReactions, delta));
        // Cập nhật gộp trong một lệnh để tránh lệch tổng khi có concurrent update.

        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Tăng/giảm số lượng comment của bài viết.
    /// Luồng xử lý: cập nhật atomic trường comments_count bằng toán tử Inc.
    /// </summary>
    public async Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update.Inc(x => x.CommentsCount, delta);
        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Cập nhật trạng thái attach media cho bài viết.
    /// Luồng xử lý: validate status hợp lệ, rồi set trạng thái/lỗi/updated_at theo post id.
    /// </summary>
    public async Task UpdateMediaAttachStatusAsync(
        string postId,
        string status,
        string? lastError = null,
        CancellationToken cancellationToken = default)
    {
        if (!MediaUploadConstants.IsCommunityEntityMediaAttachStatus(status))
        {
            throw new ArgumentException("Community media attach status không hợp lệ.", nameof(status));
        }

        var normalizedStatus = status.Trim().ToLowerInvariant();
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        var update = Builders<CommunityPostDocument>.Update
            .Set(x => x.MediaAttachStatus, normalizedStatus)
            .Set(x => x.MediaAttachLastError, string.IsNullOrWhiteSpace(lastError) ? null : lastError.Trim())
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Map document Mongo sang DTO dùng cho tầng Application/API.
    /// Luồng xử lý: copy đầy đủ field hiển thị và fallback reactions_count rỗng để tránh null reference ở caller.
    /// </summary>
    private static CommunityPostDto MapToDto(CommunityPostDocument doc)
    {
        return new CommunityPostDto
        {
            Id = doc.Id,
            AuthorId = doc.AuthorId,
            AuthorDisplayName = doc.AuthorDisplayName,
            AuthorAvatarUrl = doc.AuthorAvatarUrl,
            Content = doc.Content,
            Visibility = doc.Visibility,
            ReactionsCount = doc.ReactionsCount ?? new Dictionary<string, int>(),
            // Edge case: dữ liệu cũ có thể thiếu reactions_count, fallback dictionary rỗng để ổn định response.
            TotalReactions = doc.TotalReactions,
            CommentsCount = doc.CommentsCount,
            MediaAttachStatus = doc.MediaAttachStatus,
            MediaAttachLastError = doc.MediaAttachLastError,
            IsDeleted = doc.IsDeleted,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
