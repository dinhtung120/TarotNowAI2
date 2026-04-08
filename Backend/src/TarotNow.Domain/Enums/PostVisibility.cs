
namespace TarotNow.Domain.Enums;

// Tập hằng mức độ hiển thị của bài viết cộng đồng.
public static class PostVisibility
{
    // Công khai cho mọi người.
    public const string Public = "public";

    // Chỉ bạn bè có thể xem.
    public const string FriendsOnly = "friends_only";

    // Chỉ chủ bài viết xem được.
    public const string Private = "private";
}
