
namespace TarotNow.Domain.Enums;

// Tập hằng kết quả xử lý moderation cho nội dung cộng đồng.
public static class ModerationResult
{
    // Cảnh báo người dùng vi phạm.
    public const string Warn = "warn";

    // Gỡ bài viết vi phạm.
    public const string RemovePost = "remove_post";

    // Đóng băng tài khoản vi phạm.
    public const string FreezeAccount = "freeze";

    // Không áp dụng hành động xử lý.
    public const string NoAction = "no_action";
}
