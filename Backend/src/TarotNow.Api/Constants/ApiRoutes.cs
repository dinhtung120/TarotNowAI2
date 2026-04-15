namespace TarotNow.Api.Constants;

/// <summary>
/// Tập trung định nghĩa route để controller/hub dùng thống nhất một chuẩn URL.
/// Lý do: tránh lệch endpoint giữa attribute route, client và tài liệu API.
/// </summary>
public static class ApiRoutes
{
    // Tiền tố chung cho toàn bộ route versioned của API.
    private const string Prefix = "api/" + ApiVersions.Segment;

    // Mẫu route mặc định cho controller-based API.
    public const string Controller = Prefix + "/[controller]";

    // Nhóm route quản trị chung.
    public const string Admin = Prefix + "/admin";

    // Nhóm route quản trị tranh chấp.
    public const string AdminDisputes = Admin + "/disputes";

    // Nhóm route quản trị cộng đồng.
    public const string AdminCommunity = Admin + "/community";

    // Nhóm route xác thực và phiên đăng nhập.
    public const string Auth = Prefix + "/auth";

    // Nhóm route nghiệp vụ trải bài.
    public const string Reading = Prefix + "/reading";

    // Nhóm route quản lý phiên đọc bài.
    public const string Sessions = Prefix + "/sessions";

    // Nhóm route hội thoại người dùng và reader.
    public const string Conversations = Prefix + "/conversations";

    // Nhóm route tính năng cộng đồng.
    public const string Community = Prefix + "/community";

    // Endpoint SignalR cho chat realtime.
    public const string ChatHub = Prefix + "/chat";

    // Nhóm route nạp tiền.
    public const string Deposits = Prefix + "/deposits";

    // Nhóm route pháp lý và điều khoản.
    public const string Legal = Prefix + "/legal";

    // Nhóm route hồ sơ reader.
    public const string Reader = Prefix + "/reader";

    // Nhóm route báo cáo vi phạm/sự cố.
    public const string Reports = Prefix + "/reports";

    // Nhóm route hồ sơ người dùng.
    public const string Profile = Prefix + "/profile";

    // Nhóm route rút tiền.
    public const string Withdrawal = Prefix + "/withdrawal";

    // Nhóm route check-in hằng ngày.
    public const string CheckIn = Prefix + "/checkin";

    // Nhóm route lấy ngữ cảnh người dùng hiện tại.
    public const string UserContext = Prefix + "/user-context";

    // Gộp dữ liệu nhẹ cho navbar / snapshot "me".
    public const string Me = Prefix + "/me";

    // Trang chủ marketing (snapshot công khai).
    public const string Home = Prefix + "/home";

    // Nhóm route gamification phía người dùng.
    public const string Gamification = Prefix + "/gamification";

    // Nhóm route gamification phía quản trị.
    public const string AdminGamification = Admin + "/gamification";

    // Nhóm route gacha.
    public const string Gacha = Prefix + "/gacha";

    // Nhóm route kho đồ tarot.
    public const string Inventory = Prefix + "/inventory";

    // Route tuyệt đối cho danh mục reader, dùng khi cần leading slash rõ ràng.
    public const string ReadersAbsolute = "/" + Prefix + "/readers";
}
