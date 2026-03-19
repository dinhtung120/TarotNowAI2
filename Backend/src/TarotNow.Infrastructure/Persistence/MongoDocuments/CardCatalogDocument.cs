/*
 * ===================================================================
 * FILE: CardCatalogDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa cấu trúc (schema) cho Collection "cards_catalog" trên MongoDB.
 *   Bộ bài Tarot có 78 lá:
 *     - Major Arcana (Bộ Ẩn Lớn): 22 lá (ID 0-21), ví dụ: The Fool, The Magician, Death...
 *     - Minor Arcana (Bộ Ẩn Nhỏ): 56 lá (ID 22-77), chia thành 4 chất (suit):
 *       Wands (Gậy/Lửa), Cups (Cốc/Nước), Swords (Kiếm/Gió), Pentacles (Tiền/Đất).
 *
 *   Dữ liệu này là DỮ LIỆU TĨNH (static/seed data):
 *   → Được nạp sẵn vào DB bằng script seed_cards.js khi cài đặt hệ thống.
 *   → Hiếm khi thay đổi (chỉ khi cập nhật mô tả ý nghĩa lá bài).
 *
 *   TẠI SAO DÙNG MONGODB?
 *   → Mỗi lá bài có cấu trúc lồng phức tạp (tên đa ngôn ngữ, ý nghĩa xuôi/ngược với keywords).
 *   → MongoDB lưu JSON tự nhiên, không cần thiết kế nhiều bảng con như PostgreSQL.
 *   → Đọc 1 lá bài = 1 query duy nhất, không cần JOIN.
 *
 *   TẠI SAO TẠO CLASS C# RIÊNG (POCO) THAY VÌ DÙNG DOMAIN ENTITY?
 *   → Clean Architecture: Domain Entity không được phụ thuộc vào [BsonElement], [BsonId].
 *   → Các attribute MongoDB chỉ nằm ở tầng Infrastructure.
 *   → Khi cần chuyển đổi: dùng Mapper (AutoMapper) để map Document ↔ Domain Entity.
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Đại diện cho 1 lá bài Tarot trong collection "cards_catalog" trên MongoDB.
/// Tổng cộng có 78 document trong collection này (mỗi lá bài = 1 document).
/// </summary>
public class CardCatalogDocument
{
    /// <summary>
    /// ID cố định của lá bài, là số nguyên từ 0 đến 77.
    /// KHÔNG dùng ObjectId mặc định vì ID này đã được định sẵn trong script seed_cards.js.
    /// Major Arcana: 0-21 (22 lá). Minor Arcana: 22-77 (56 lá).
    /// [BsonId] = đánh dấu đây là trường khóa chính (_id) trong MongoDB.
    /// </summary>
    [BsonId]
    public int Id { get; set; }

    /// <summary>
    /// Mã code ổn định (không thay đổi) dùng làm key logic trong code.
    /// Ví dụ: "the_fool", "ace_of_wands", "king_of_cups".
    /// Tại sao cần code riêng ngoài Id?
    /// → Id là số (0-77), khó đọc/debug. Code là chuỗi có nghĩa, dễ hiểu.
    /// → Có Unique Index trong MongoDB → đảm bảo không có 2 lá bài trùng code.
    /// </summary>
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Tên lá bài được dịch ra 3 ngôn ngữ: Tiếng Việt, Tiếng Anh, Tiếng Trung.
    /// Ví dụ: { vi: "Kẻ Khờ", en: "The Fool", zh: "愚者" }
    /// Sử dụng class con LocalizedName để chứa 3 trường vi/en/zh.
    /// Hệ thống i18n sẽ chọn ngôn ngữ phù hợp theo locale của người dùng.
    /// </summary>
    [BsonElement("name")]
    public LocalizedName Name { get; set; } = new();

    /// <summary>
    /// Phân loại bài: "major" (Ẩn Lớn) hoặc "minor" (Ẩn Nhỏ).
    /// → Major Arcana: 22 lá mang ý nghĩa lớn về cuộc đời (số phận, chuyển đổi lớn).
    /// → Minor Arcana: 56 lá phản ánh các sự kiện hàng ngày.
    /// </summary>
    [BsonElement("arcana")]
    public string Arcana { get; set; } = string.Empty;

    /// <summary>
    /// Chất (suit) của lá bài, chỉ có ở Minor Arcana:
    ///   - "wands" (Gậy) = nguyên tố Lửa = năng lượng, sáng tạo, tham vọng
    ///   - "cups" (Cốc) = nguyên tố Nước = cảm xúc, tình yêu, trực giác
    ///   - "swords" (Kiếm) = nguyên tố Gió = tư duy, xung đột, quyết định
    ///   - "pentacles" (Tiền) = nguyên tố Đất = tài chính, sức khỏe, vật chất
    /// Major Arcana không có suit → giá trị null.
    /// [BsonIgnoreIfNull] = không ghi trường này vào DB nếu null → tiết kiệm dung lượng.
    /// </summary>
    [BsonElement("suit")]
    [BsonIgnoreIfNull]
    public string? Suit { get; set; }

    /// <summary>
    /// Số thứ tự của lá bài trong bộ.
    /// Major Arcana: 0 (The Fool) đến 21 (The World).
    /// Minor Arcana: 1 (Ace) đến 14 (King) cho mỗi suit.
    /// </summary>
    [BsonElement("number")]
    public int Number { get; set; }

    /// <summary>
    /// Nguyên tố liên kết với lá bài: "fire" (Lửa), "water" (Nước), "air" (Gió), "earth" (Đất).
    /// Mỗi suit tương ứng 1 nguyên tố. Major Arcana có thể có nguyên tố riêng tùy truyền thống.
    /// </summary>
    [BsonElement("element")]
    public string Element { get; set; } = string.Empty;

    /// <summary>
    /// Ý nghĩa của lá bài theo 2 hướng: xuôi (upright) và ngược (reversed).
    /// Khi bốc bài Tarot, lá bài có thể rơi xuôi hoặc ngược → ý nghĩa khác nhau.
    /// Sử dụng class con CardMeanings chứa 2 object Upright và Reversed.
    /// </summary>
    [BsonElement("meanings")]
    public CardMeanings Meanings { get; set; } = new();

    /// <summary>Thời điểm lá bài được thêm vào catalog (seed data).</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm cập nhật cuối cùng (ví dụ: sửa mô tả ý nghĩa).</summary>
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Tên lá bài đa ngôn ngữ — hỗ trợ 3 locale bắt buộc theo đặc tả i18n của hệ thống.
/// Fallback chain: locale người dùng → en (tiếng Anh) nếu locale không có.
/// Trong MongoDB lưu dạng: { "vi": "...", "en": "...", "zh": "..." }
/// </summary>
public class LocalizedName
{
    /// <summary>Tên tiếng Việt. Ví dụ: "Kẻ Khờ", "Vua Cốc".</summary>
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;

    /// <summary>Tên tiếng Anh. Ví dụ: "The Fool", "King of Cups".</summary>
    [BsonElement("en")] public string En { get; set; } = string.Empty;

    /// <summary>Tên tiếng Trung. Ví dụ: "愚者", "圣杯国王".</summary>
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}

/// <summary>
/// Ý nghĩa lá bài theo 2 hướng: xuôi (upright) và ngược (reversed).
/// Mỗi hướng có danh sách từ khóa (keywords) và mô tả chi tiết (description).
/// </summary>
public class CardMeanings
{
    /// <summary>Ý nghĩa khi lá bài xuôi (upright = đúng chiều). Thường mang nghĩa tích cực.</summary>
    [BsonElement("upright")] public MeaningDetail Upright { get; set; } = new();

    /// <summary>Ý nghĩa khi lá bài ngược (reversed = lật ngược). Thường mang nghĩa tiêu cực hoặc bị chặn.</summary>
    [BsonElement("reversed")] public MeaningDetail Reversed { get; set; } = new();
}

/// <summary>
/// Chi tiết ý nghĩa cho 1 hướng (xuôi hoặc ngược) của lá bài.
/// Keywords dùng để AI tạo prompt nhanh, Description dùng để hiển thị cho người dùng đọc.
/// </summary>
public class MeaningDetail
{
    /// <summary>
    /// Danh sách từ khóa ngắn gọn. Ví dụ: ["Khởi đầu mới", "Phiêu lưu", "Tự do"].
    /// AI sử dụng keywords này trong prompt để tạo bài giải thích cho người dùng.
    /// </summary>
    [BsonElement("keywords")] public List<string> Keywords { get; set; } = new();

    /// <summary>
    /// Mô tả đầy đủ ý nghĩa lá bài (dạng đoạn văn).
    /// Hiển thị trên UI khi người dùng muốn đọc chi tiết ý nghĩa gốc (không qua AI).
    /// </summary>
    [BsonElement("description")] public string Description { get; set; } = string.Empty;
}
