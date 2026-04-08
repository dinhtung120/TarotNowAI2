

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document danh mục lá bài tarot chuẩn hệ thống.
public class CardCatalogDocument
{
    // Khóa số nguyên ổn định để map với dữ liệu card nội bộ.
    [BsonId]
    public int Id { get; set; }

    // Mã card duy nhất dùng cho index và lookup nghiệp vụ.
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    // Tên hiển thị đa ngôn ngữ của lá bài.
    [BsonElement("name")]
    public LocalizedName Name { get; set; } = new();

    // Nhóm arcana (major/minor).
    [BsonElement("arcana")]
    public string Arcana { get; set; } = string.Empty;

    // Suit của minor arcana, null với major arcana.
    [BsonElement("suit")]
    [BsonIgnoreIfNull]
    public string? Suit { get; set; }

    // Số thứ tự trong bộ bài.
    [BsonElement("number")]
    public int Number { get; set; }

    // Thuộc tính nguyên tố dùng cho bộ lọc/diễn giải.
    [BsonElement("element")]
    public string Element { get; set; } = string.Empty;

    // Ảnh minh họa card.
    [BsonElement("image_url")]
    [BsonIgnoreIfNull]
    public string? ImageUrl { get; set; }

    // Ý nghĩa upright/reversed phục vụ đọc bài.
    [BsonElement("meanings")]
    public CardMeanings Meanings { get; set; } = new();

    // Thời điểm tạo bản ghi card.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật bản ghi card.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
// Tên card theo từng locale hỗ trợ.
public class LocalizedName
{
    // Tên tiếng Việt.
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;

    // Tên tiếng Anh.
    [BsonElement("en")] public string En { get; set; } = string.Empty;

    // Tên tiếng Trung.
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
// Cặp ý nghĩa thuận/ngược của card.
public class CardMeanings
{
    // Ý nghĩa khi lá bài ở trạng thái upright.
    [BsonElement("upright")] public MeaningDetail Upright { get; set; } = new();

    // Ý nghĩa khi lá bài ở trạng thái reversed.
    [BsonElement("reversed")] public MeaningDetail Reversed { get; set; } = new();
}

[BsonIgnoreExtraElements]
// Chi tiết diễn giải một trạng thái card.
public class MeaningDetail
{
    // Từ khóa ngắn gọn dùng cho UI/tagging.
    [BsonElement("keywords")] public List<string> Keywords { get; set; } = new();

    // Mô tả diễn giải đầy đủ.
    [BsonElement("description")] public string Description { get; set; } = string.Empty;
}
