using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReaderProfileRepository
{
    private static BsonDocument BuildStatusPriorityExpression()
    {
        return new BsonDocument("$switch", new BsonDocument
        {
            {
                "branches", new BsonArray
                {
                    CreateStatusPriorityBranch(ReaderOnlineStatus.AcceptingQuestions, 0),
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Online, 1),
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Away, 2),
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Offline, 3)
                }
            },
            { "default", 4 }
        });
    }

    private static BsonDocument CreateStatusPriorityBranch(string status, int priority)
    {
        return new BsonDocument
        {
            { "case", new BsonDocument("$eq", new BsonArray { "$status", status }) },
            { "then", priority }
        };
    }

    private static FilterDefinition<ReaderProfileDocument> BuildDirectoryFilter(
        string? specialty,
        string? status,
        string? searchTerm)
    {
        var filterBuilder = Builders<ReaderProfileDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (string.IsNullOrEmpty(specialty) == false)
        {
            filter = filterBuilder.And(filter, filterBuilder.AnyEq(r => r.Specialties, specialty));
        }

        if (string.IsNullOrEmpty(status) == false)
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, status));
        }

        if (string.IsNullOrEmpty(searchTerm))
        {
            return filter;
        }

        var regex = new BsonRegularExpression(searchTerm, "i");
        return filterBuilder.And(filter, filterBuilder.Regex(r => r.DisplayName, regex));
    }
}
