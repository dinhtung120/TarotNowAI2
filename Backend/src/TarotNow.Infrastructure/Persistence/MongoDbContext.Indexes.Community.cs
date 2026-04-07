

using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence;

public partial class MongoDbContext
{
    private void EnsureCommunityIndexes()
    {
        
        
        

        
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Descending(x => x.CreatedAt)
                .Ascending(x => x.IsDeleted),
            new CreateIndexOptions { Name = "idx_createdat_isdeleted" }));

        
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));

        
        SafeCreateIndex(CommunityPosts, new CreateIndexModel<CommunityPostDocument>(
            Builders<CommunityPostDocument>.IndexKeys
                .Ascending(x => x.Visibility)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_visibility_isdeleted_createdat" }));

        
        
        

        
        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.UserId),
            new CreateIndexOptions { Unique = true, Name = "idx_postid_userid_unique" }));

        
        SafeCreateIndex(CommunityReactions, new CreateIndexModel<CommunityReactionDocument>(
            Builders<CommunityReactionDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_createdat" }));

        
        
        

        
        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.PostId)
                .Ascending(x => x.IsDeleted)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_postid_isdeleted_createdat" }));

        
        SafeCreateIndex(CommunityComments, new CreateIndexModel<CommunityCommentDocument>(
            Builders<CommunityCommentDocument>.IndexKeys
                .Ascending(x => x.AuthorId)
                .Descending(x => x.CreatedAt),
            new CreateIndexOptions { Name = "idx_authorid_createdat" }));
    }
}
