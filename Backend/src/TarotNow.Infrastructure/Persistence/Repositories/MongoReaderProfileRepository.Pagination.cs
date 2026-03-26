using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReaderProfileRepository
{
    public async Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? specialty = null,
        string? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 12 : Math.Min(pageSize, 200);
        var filter = BuildDirectoryFilter(specialty, status, searchTerm);

        var totalCount = await _context.ReaderProfiles.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryDirectoryDocsAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);
        var mappedDocs = docs.Select(doc => BsonSerializer.Deserialize<ReaderProfileDocument>(doc));
        return (mappedDocs.Select(ToDto), totalCount);
    }

    private async Task<List<BsonDocument>> QueryDirectoryDocsAsync(
        FilterDefinition<ReaderProfileDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var statusPriority = BuildStatusPriorityExpression();

        return await _context.ReaderProfiles
            .Aggregate()
            .Match(filter)
            .AppendStage<BsonDocument>(new BsonDocument("$addFields", new BsonDocument("status_priority", statusPriority)))
            .AppendStage<BsonDocument>(new BsonDocument("$sort", new BsonDocument
            {
                { "status_priority", 1 },
                { "updated_at", -1 }
            }))
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .AppendStage<BsonDocument>(new BsonDocument("$project", new BsonDocument("status_priority", 0)))
            .As<BsonDocument>()
            .ToListAsync(cancellationToken);
    }

    private static BsonDocument BuildStatusPriorityExpression()
    {
        return new BsonDocument("$switch", new BsonDocument
        {
            {
                "branches", new BsonArray
                {
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.AcceptingQuestions }) },
                        { "then", 0 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Online }) },
                        { "then", 1 }
                    },
                    new BsonDocument
                    {
                        { "case", new BsonDocument("$eq", new BsonArray { "$status", ReaderOnlineStatus.Offline }) },
                        { "then", 2 }
                    }
                }
            },
            { "default", 3 }
        });
    }

    private static FilterDefinition<ReaderProfileDocument> BuildDirectoryFilter(
        string? specialty,
        string? status,
        string? searchTerm)
    {
        var filterBuilder = Builders<ReaderProfileDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (!string.IsNullOrEmpty(specialty))
        {
            filter = filterBuilder.And(filter, filterBuilder.AnyEq(r => r.Specialties, specialty));
        }

        if (!string.IsNullOrEmpty(status))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, status));
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var regex = new BsonRegularExpression(searchTerm, "i");
            filter = filterBuilder.And(filter, filterBuilder.Regex(r => r.DisplayName, regex));
        }

        return filter;
    }
}
