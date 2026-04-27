using MongoDB.Driver;
using System;
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
        if (delta == 0)
        {
            return;
        }

        var reactionField = $"reactions_count.{reactionType}";
        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        if (delta < 0)
        {
            filter = Builders<CommunityPostDocument>.Filter.And(
                filter,
                Builders<CommunityPostDocument>.Filter.Gt(reactionField, 0),
                Builders<CommunityPostDocument>.Filter.Gt(x => x.TotalReactions, 0));
        }

        var update = Builders<CommunityPostDocument>.Update.Combine(
            Builders<CommunityPostDocument>.Update.Inc(reactionField, delta),
            Builders<CommunityPostDocument>.Update.Inc(x => x.TotalReactions, delta));
        // Cập nhật gộp trong một lệnh để tránh lệch tổng khi có concurrent update.

        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Swap bộ đếm reaction old -> new trong một câu lệnh atomic.
    /// Luồng xử lý: chỉ swap khi old count > 0, giảm old và tăng new; total_reactions giữ nguyên.
    /// </summary>
    public async Task SwapReactionCountAsync(
        string postId,
        string oldReactionType,
        string newReactionType,
        CancellationToken cancellationToken = default)
    {
        if (string.Equals(oldReactionType, newReactionType, StringComparison.Ordinal))
        {
            return;
        }

        var oldField = $"reactions_count.{oldReactionType}";
        var newField = $"reactions_count.{newReactionType}";
        var filter = Builders<CommunityPostDocument>.Filter.And(
            Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId),
            Builders<CommunityPostDocument>.Filter.Gt(oldField, 0));
        var update = Builders<CommunityPostDocument>.Update.Combine(
            Builders<CommunityPostDocument>.Update.Inc(oldField, -1),
            Builders<CommunityPostDocument>.Update.Inc(newField, 1));
        await _context.CommunityPosts.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Tăng/giảm số lượng comment của bài viết.
    /// Luồng xử lý: cập nhật atomic trường comments_count bằng toán tử Inc.
    /// </summary>
    public async Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default)
    {
        if (delta == 0)
        {
            return;
        }

        var filter = Builders<CommunityPostDocument>.Filter.Eq(x => x.Id, postId);
        if (delta < 0)
        {
            filter = Builders<CommunityPostDocument>.Filter.And(
                filter,
                Builders<CommunityPostDocument>.Filter.Gt(x => x.CommentsCount, 0));
        }

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
