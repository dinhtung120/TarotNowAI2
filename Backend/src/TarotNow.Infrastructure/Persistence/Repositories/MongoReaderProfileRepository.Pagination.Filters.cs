using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial dựng filter/sort expression cho directory reader.
public partial class MongoReaderProfileRepository
{
    /// <summary>
    /// Tạo biểu thức ưu tiên trạng thái để sort reader directory.
    /// Luồng xử lý: dùng $switch gán online=0, busy=1, offline=2 và default=3.
    /// </summary>
    private static BsonDocument BuildStatusPriorityExpression()
    {
        return new BsonDocument("$switch", new BsonDocument
        {
            {
                "branches", new BsonArray
                {
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Online, 0),
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Busy, 1),
                    CreateStatusPriorityBranch(ReaderOnlineStatus.Offline, 2)
                }
            },
            { "default", 3 }
        });
    }

    /// <summary>
    /// Tạo một nhánh case-then cho biểu thức ưu tiên trạng thái.
    /// Luồng xử lý: so sánh trường status với giá trị đầu vào và trả priority tương ứng.
    /// </summary>
    private static BsonDocument CreateStatusPriorityBranch(string status, int priority)
    {
        return new BsonDocument
        {
            { "case", new BsonDocument("$eq", new BsonArray { "$status", status }) },
            { "then", priority }
        };
    }

    /// <summary>
    /// Dựng filter cho API directory reader.
    /// Luồng xử lý: luôn lọc is_deleted=false, thêm điều kiện specialty/status và search display_name khi có input.
    /// </summary>
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
            // AnyEq đảm bảo match đúng phần tử specialty trong mảng chuyên môn.
        }

        if (string.IsNullOrEmpty(status) == false)
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, status));
        }

        if (string.IsNullOrEmpty(searchTerm))
        {
            return filter;
            // Không có từ khóa tìm kiếm thì giữ filter hiện tại để tận dụng index tốt hơn.
        }

        var regex = new BsonRegularExpression(searchTerm, "i");
        return filterBuilder.And(filter, filterBuilder.Regex(r => r.DisplayName, regex));
        // Regex không phân biệt hoa thường cho trải nghiệm tìm kiếm tên reader.
    }
}
