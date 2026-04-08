
namespace TarotNow.Domain.Enums;

// Tập hằng trạng thái vòng đời item câu hỏi có ràng buộc escrow.
public static class QuestionItemStatus
{
    // Item đang chờ xử lý ban đầu.
    public const string Pending = "pending";

    // Item đã được chấp nhận.
    public const string Accepted = "accepted";

    // Tiền của item đã được giải ngân.
    public const string Released = "released";

    // Item đã hoàn tiền cho người trả.
    public const string Refunded = "refunded";

    // Item đang trong trạng thái tranh chấp.
    public const string Disputed = "disputed";

    /// <summary>
    /// Kiểm tra status item có nằm trong tập trạng thái hợp lệ của hệ thống hay không.
    /// Luồng xử lý: so khớp chuỗi đầu vào với toàn bộ hằng trạng thái đã định nghĩa.
    /// </summary>
    public static bool IsValid(string s) =>
        s is Pending or Accepted or Released or Refunded or Disputed;
}
